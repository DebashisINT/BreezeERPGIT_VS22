<%@ Page Language="C#" Title="Registration" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/Erp.Master" CodeBehind="HRrecruitmentagent_Registration.aspx.cs" Inherits="ERP.OMS.Management.Master.HRrecruitmentagent_Registration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    

    <style type="text/css">
        a {
            color: #000;
        }

        .dxpcLite_PlasticBlue .dxpc-header, .dxdpLite_PlasticBlue .dxpc-header {
            color: #FFF !important;
        }

        .unitPriceColumn {
        }

        .formatcss {
            font: bold;
            color: red;
        }

        .dxeDisabled_PlasticBlue {
            opacity: 0.4;
        }
    </style>


    <script language="javascript" type="text/javascript">

        var FieldX = null;
        //unitPriceColumn
        function OnFieldXChanged1(cmbFieldX) {
            var comboValue = cmbFieldX.GetValue();


            // membership.PerformCallback(comboValue);
            //  var coulmmn = membership.GetEditor('crg_memExpDate1').SetVisible(false);
            if (comboValue != null && comboValue != "") {
                FieldX = comboValue;
                if (comboValue == "Lifetime") {
                    var k = $('td.unitPriceColumn');
                    var coulmmn = membership.GetEditor('crg_memExpDate');
                    var coulmmnh = membership.GetEditor('crg_memExpDate').SetVisible(false);
                    $('#gridMembership_DXPEForm_efnew_DXEFL_4').css('display', 'none');
                    // $('td.unitPriceColumn').css('display', 'none');
                    //membership.GetEditor(crg_memExpDate).SetClientVisible(false);
                }
                else if (comboValue == "LifeTime") {
                    // $('td.unitPriceColumn').css('display', 'none');
                    // membership.GetEditor(crg_memExpDate).SetClientVisible(false);
                }
                else {
                    // $('td.unitPriceColumn').css('display', 'block');
                    //membership.GetEditor(crg_memExpDate).SetClientVisible(true);
                }
            }


            // membership.GetEditor(&quot;crg_memExpDate&quot;).SetClientVisible(false);
            //else if (editor != null) {
            //    editor.PerformCallback(cmbFieldX.GetValue());
            //}
        }

        function OnFieldYChanged(cmbFieldY) {
            grid.PerformCallback();
        }

        //function OnFieldYEndCallback(s, e) {
        //    if (FieldX) {
        //        grid.GetEditor("FieldY").PerformCallback(FieldX);
        //        FieldX = null;
        //    }
        //}
        function ChangeDateFormat_CalToDB(obj) {


            var SelectedDate = new Date(obj);

            var monthnumber = SelectedDate.getMonth() + 1;
            jAlert(monthnumber);
            var monthday = SelectedDate.getDate();

            //var year = SelectedDate.getYear();
            var year = SelectedDate.getFullYear();

            var changedDateValue = year + '-' + monthnumber + '-' + monthday;

            return changedDateValue;

        }
        ////=======
        //		sEntryDate=cdtentrydate.GetDate(); 
        //                sEntryDate=ChangeDateFormat_CalToDB(sEntryDate); 

        //===================================
        function ChangeDateFormat_SetCalenderValue(obj) {
            var SelectedDate = new Date(obj);
            var monthnumber = SelectedDate.getMonth();
            var monthday = SelectedDate.getDate();
            //var year = SelectedDate.getYear(); 
            var year = SelectedDate.getFullYear();
            var changeDateValue = new Date(year, monthnumber, monthday);
            return changeDateValue;
        }

        //======= 
        //                var currentDate=ChangeDateFormat_SetCalenderValue('<%=Session["NowDate"]%>');
        //                cDtAddRateDate.SetDate(currentDate);      



        function SignOff() {
            //window.parent.SignOff()
        }
        EditPopUpID = '';
        function fn_btnCancel() {
            cPopup_StatutoryRegistration.Hide();
        }
        function ForcePostBack() {
            // alert("test");
            document.getElementById("hdnBannedPAN").value = 'n';
            document.getElementById("hdnDuplicatePAN").value = 'n';
            __doPostBack();
        }
        function Reset() {
            document.getElementById('<%=hiddenedit.ClientID%>').value = '';
            cchkPan.SetChecked(false);
            cchkPan.SetEnabled(true);
            ccmbSR.SetSelectedIndex(0);
            cCmbProofType.SetSelectedIndex(0);
            HideShow('PanCheck', 'S');
            //cCmbProofNumber.SetText('');
            ctxtNumber.SetText('');
            ctxtIssuePlace.SetText('');
            cdtValid.SetDate(new Date());
            cdtIssue.SetDate(new Date());

        }
        function fn_PopOpen() {
            Reset();
            cPopup_StatutoryRegistration.Show();
            fn_ComboIndexChange('1');

            cbtnSave_Registration.SetText('Save');
            ctxtNumber.SetText('');
            ctxtIssuePlace.SetText('Not Applicable');
            ctxtRegAuthName.SetText('Not Applicable');
            cCmbProofNumber.SetText('Not Applicable');
            ctxtNumber.SetEnabled(true);
            ccmbSR.SetEnabled(true);
            cCmbProofType.SetEnabled(false);
            cCmbProofNumber.SetEnabled(false);
            ctxtIssuePlace.SetEnabled(false);
            cdtValid.SetEnabled(false);
            cdtIssue.SetEnabled(false);
            ccmbVerify.SetEnabled(true);
            ctxtRegAuthName.SetEnabled(false);

        }
        function fn_PopEditOpen(keyValue) {

            grid.PerformCallback("Edit~" + keyValue);

        }
        function fn_Delete(keyValue) {
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    document.getElementById("hdnBannedPAN").value = 'n';
                    document.getElementById("hdnDuplicatePAN").value = 'n';
                    grid.PerformCallback("DeleteItem~" + keyValue);
                }
            });


        }
        function fn_CheckChange(s, e) {
            if (s.GetChecked()) {
                ctxtNumber.SetText('PAN_EXEMPT');
                ctxtIssuePlace.SetText('');
                ctxtRegAuthName.SetText('Not Applicable');
                cCmbProofNumber.SetText('');
                //
                ctxtNumber.SetEnabled(false);
                ccmbSR.SetEnabled(true);
                cCmbProofType.SetEnabled(true);
                cCmbProofNumber.SetEnabled(true);
                ctxtIssuePlace.SetEnabled(true);
                cdtValid.SetEnabled(true);
                cdtIssue.SetEnabled(true);
                ccmbVerify.SetEnabled(true);
                ctxtRegAuthName.SetEnabled(false)

                cCmbProofType.SetValue(1);

            }
            else {
                ctxtNumber.SetText('');
                ctxtIssuePlace.SetText('Not Applicable');
                ctxtRegAuthName.SetText('Not Applicable');
                cCmbProofNumber.SetText('Not Applicable');
                ctxtNumber.SetEnabled(true);
                ccmbSR.SetEnabled(true);
                cCmbProofType.SetEnabled(false);
                cCmbProofNumber.SetEnabled(false);
                ctxtIssuePlace.SetEnabled(false);
                cdtValid.SetEnabled(false);
                cdtIssue.SetEnabled(false);
                ccmbVerify.SetEnabled(true);
                ctxtRegAuthName.SetEnabled(false)
            }
        }
        function fn_ComboIndexChange(ele) {

            ctxtNumber.SetText('');
            ctxtNumber.SetEnabled(true);
            cdtValid.SetDate(new Date());
            cdtIssue.SetDate(new Date());

            //alert(ele);

            document.getElementById('hfSelectedType').value = ele;

            if (ele == '1') {
                ctxtNumber.SetText('');
                lbformat.SetText('Exa: XXXXX9999X');
                lbformat.SetVisible(true);
                lbformat.SetVisible(true);
                ctxtNumber.SetEnabled(true);
                ccmbSR.SetEnabled(true);
                cCmbProofType.SetEnabled(false);
                ctxtIssuePlace.SetText('Not Applicable');
                cCmbProofNumber.SetEnabled(false);
                ctxtIssuePlace.SetEnabled(false);
                cdtValid.SetEnabled(false);
                cdtIssue.SetEnabled(false);
                ccmbVerify.SetEnabled(true);
                ctxtRegAuthName.SetEnabled(false);
                HideShow('PanCheck', 'S');

            }

            else if ((ele == '5') || (ele == '3')) {

                ctxtNumber.SetText('');
                ctxtIssuePlace.SetText('');
                ctxtNumber.SetEnabled(true);
                ccmbSR.SetEnabled(true);
                cCmbProofType.SetEnabled(false);
                cCmbProofNumber.SetEnabled(false);
                ctxtIssuePlace.SetEnabled(true);
                cdtValid.SetEnabled(false);
                cdtIssue.SetEnabled(true);
                ccmbVerify.SetEnabled(true);
                ctxtRegAuthName.SetEnabled(false);
                HideShow('PanCheck', 'H');

            }

            else if ((ele == '2') || (ele == '4') || (ele == '6') || (ele == '9') || (ele == '11')) {

                lbformat.SetText('');
                lbformat.SetVisible(false);
                ctxtNumber.SetText('');
                ctxtIssuePlace.SetText('');
                ctxtRegAuthName.SetText('Not Applicable');
                cCmbProofNumber.SetText('Not Applicable');
                ctxtNumber.SetEnabled(true);
                ccmbSR.SetEnabled(true);
                cCmbProofType.SetEnabled(false);
                cCmbProofNumber.SetEnabled(false);
                ctxtIssuePlace.SetEnabled(true);
                cdtValid.SetEnabled(true);
                cdtIssue.SetEnabled(true);
                ccmbVerify.SetEnabled(true);
                ctxtRegAuthName.SetEnabled(false);
                HideShow('PanCheck', 'H');
            }
            else if ((ele == '7') || (ele == '8')) {
                ctxtNumber.SetText('');
                lbformat.SetText('');
                lbformat.SetVisible(false);
                ctxtIssuePlace.SetText('');
                ctxtRegAuthName.SetText('Not Applicable');
                cCmbProofNumber.SetText('Not Applicable');
                ctxtNumber.SetEnabled(true);
                ccmbSR.SetEnabled(true);
                cCmbProofType.SetEnabled(false);
                cCmbProofNumber.SetEnabled(false);
                ctxtIssuePlace.SetEnabled(true);
                cdtValid.SetEnabled(false);
                cdtIssue.SetEnabled(true);
                ccmbVerify.SetEnabled(true);
                ctxtRegAuthName.SetEnabled(false);
                HideShow('PanCheck', 'H');
            }

            else if ((ele == '10') || (ele == '13')) {
                lbformat.SetText('');
                lbformat.SetVisible(false);
                ctxtNumber.SetText('');
                ctxtIssuePlace.SetText('');
                ctxtRegAuthName.SetText('');
                cCmbProofNumber.SetText('Not Applicable');
                ctxtNumber.SetEnabled(true);
                ccmbSR.SetEnabled(true);
                cCmbProofType.SetEnabled(false);
                cCmbProofNumber.SetEnabled(false);
                ctxtIssuePlace.SetEnabled(true);
                cdtValid.SetEnabled(false);
                cdtIssue.SetEnabled(true);
                ccmbVerify.SetEnabled(false);
                ctxtRegAuthName.SetEnabled(true);
                HideShow('PanCheck', 'H');

            }
            else if (ele == '12') {
                lbformat.SetText('');
                lbformat.SetVisible(false);
                ctxtNumber.SetText('');
                ctxtIssuePlace.SetText('');
                ctxtRegAuthName.SetText('');
                ctxtRegAuthName.SetText('Not Applicable');
                ctxtRegAuthName.SetEnabled(false);
                cCmbProofNumber.SetText('Not Applicable');
                ctxtNumber.SetEnabled(true);
                ccmbSR.SetEnabled(true);
                cCmbProofType.SetEnabled(false);
                cCmbProofNumber.SetEnabled(false);
                ctxtIssuePlace.SetEnabled(true);
                cdtValid.SetEnabled(false);
                cdtIssue.SetEnabled(true);
                ccmbVerify.SetEnabled(true);
                //ctxtRegAuthName.SetEnabled(true);
                HideShow('PanCheck', 'H');

            }
            else if (ele == '14') {
                lbformat.SetText('');
                lbformat.SetVisible(false);
                ctxtNumber.SetText('');
                ctxtNumber.SetEnabled(true);
                ccmbSR.SetEnabled(true);
                cCmbProofType.SetEnabled(false);
                ctxtIssuePlace.SetText('Not Applicable');
                cCmbProofNumber.SetEnabled(false);
                ctxtIssuePlace.SetEnabled(false);
                cdtValid.SetEnabled(false);
                cdtIssue.SetEnabled(false);
                ccmbVerify.SetEnabled(true);
                ctxtRegAuthName.SetEnabled(false);
                HideShow('PanCheck', 'H');
            }
                //.............. Code Added by Sam on 17102016........................... 

            else if ((ele == '15') || (ele == '16') || (ele == '17') || (ele == '18') || (ele == '19') || (ele == '20') || (ele == '21')) {

                if (ele == '15') {
                    lbformat.SetText('99999999999');
                    lbformat.SetVisible(true);
                }
                if (ele == '17') {
                    lbformat.SetText('XXXXX9999XXX999');
                    lbformat.SetVisible(true);
                }
                if (ele == '18') {
                    lbformat.SetText('XXXXX9999XXX999');
                    lbformat.SetVisible(true);
                }
                if (ele == '19') {
                    lbformat.SetText('');
                    lbformat.SetVisible(false);
                }
                if (ele == '20') {
                    lbformat.SetText('');
                    lbformat.SetVisible(false);
                }
                if (ele == '21') {
                    lbformat.SetText('');
                    lbformat.SetVisible(false);
                }

                ctxtNumber.SetText('');
                ctxtNumber.SetEnabled(true);
                ccmbSR.SetEnabled(true);
                cCmbProofType.SetEnabled(false);
                ctxtIssuePlace.SetText('Not Applicable');
                cCmbProofNumber.SetEnabled(false);
                ctxtIssuePlace.SetEnabled(false);
                cdtValid.SetEnabled(false);
                cdtIssue.SetEnabled(false);
                ccmbVerify.SetEnabled(true);
                ctxtRegAuthName.SetEnabled(false);
                HideShow('PanCheck', 'H');
            }


            //.............. Code Above Added by Sam on 17102016........................... 

        }
        function btnSave_Registration() {

            var type = ccmbSR.GetText();

            //        var validuntildt=ChangeDateFormat_CalToDB(cdtValid.GetDate());
            //        var issuedate=ChangeDateFormat_CalToDB(cdtIssue.GetDate());
            var validuntildt = cdtValid.GetDate();
            var issuedate = cdtIssue.GetDate();



            if (type == "VAT Regn No") {
                //var PAN = document.getElementById('ASPxPageControl1_ASPxPageControl2_Popup_StatutoryRegistration_txtNumber_I').value;
                var PAN = document.getElementById('txtNumber_I').value;
                var regex = /^[0-9]{11}$/;
                if (!regex.test(PAN)) {
                    jAlert("Please enter valid VAT No");
                    return false;
                }
                else {
                    if (GetObjectID('hiddenedit').value == '')
                        grid.PerformCallback('savenew~' + type + '~' + PAN + '~' + cCmbProofType.GetValue() + '~' + PAN + '~' + ctxtNumber.GetText() + '~' + ctxtIssuePlace.GetText() + '~' + validuntildt + '~' + issuedate + '~' + ccmbVerify.GetText());
                    else
                        grid.PerformCallback('saveold~' + type + '~' + PAN + '~' + cCmbProofType.GetValue() + '~' + PAN + '~' + ctxtNumber.GetText() + '~' + ctxtIssuePlace.GetText() + '~' + validuntildt + '~' + issuedate + '~' + ccmbVerify.GetText() + '~' + GetObjectID('hiddenedit').value);
                }
            }
            else if (type == "CST Regn No") {
                //var PAN = document.getElementById('ASPxPageControl1_ASPxPageControl2_Popup_StatutoryRegistration_txtNumber_I').value;
                var PAN = document.getElementById('txtNumber_I').value;
                var regex = /^[a-zA-Z0-9]{11}$/;
                if (!regex.test(PAN)) {
                    jAlert("Please enter valid CST Regn No");
                    return false;
                }
                else {
                    if (GetObjectID('hiddenedit').value == '')
                        grid.PerformCallback('savenew~' + type + '~' + PAN + '~' + cCmbProofType.GetValue() + '~' + PAN + '~' + ctxtNumber.GetText() + '~' + ctxtIssuePlace.GetText() + '~' + validuntildt + '~' + issuedate + '~' + ccmbVerify.GetText());
                    else
                        grid.PerformCallback('saveold~' + type + '~' + PAN + '~' + cCmbProofType.GetValue() + '~' + PAN + '~' + ctxtNumber.GetText() + '~' + ctxtIssuePlace.GetText() + '~' + validuntildt + '~' + issuedate + '~' + ccmbVerify.GetText() + '~' + GetObjectID('hiddenedit').value);
                }
            }
            else if (type == "SGST" || type == "IGST" || type == "CGST") {
                var PAN = document.getElementById('txtNumber_I').value;
                if (PAN.length > 0) {
                    if (GetObjectID('hiddenedit').value == '')
                        grid.PerformCallback('savenew~' + type + '~' + PAN + '~' + cCmbProofType.GetValue() + '~' + PAN + '~' + ctxtNumber.GetText() + '~' + ctxtIssuePlace.GetText() + '~' + validuntildt + '~' + issuedate + '~' + ccmbVerify.GetText());
                    else
                        grid.PerformCallback('saveold~' + type + '~' + PAN + '~' + cCmbProofType.GetValue() + '~' + PAN + '~' + ctxtNumber.GetText() + '~' + ctxtIssuePlace.GetText() + '~' + validuntildt + '~' + issuedate + '~' + ccmbVerify.GetText() + '~' + GetObjectID('hiddenedit').value);
                }
                else {
                    jAlert("Please enter Number");
                }
            }
            else if (type == "ECC Regn No") {
                var PAN = document.getElementById('txtNumber_I').value;
                var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{3})(\d{3})$/;
                var code = /([C,P,H,F,A,T,B,L,J,G])/;
                var code_chk = PAN.substring(3, 4);
                if (PAN.search(panPat) == -1) {
                    jAlert("Please enter valid ECC Regn No");
                    //Obj.focus();
                    return false;
                }
                if (code.test(code_chk) == false) {
                    jAlert("Invaild ECC Regn No.");
                    return false;
                }
                else {
                    if (GetObjectID('hiddenedit').value == '')
                        grid.PerformCallback('savenew~' + type + '~' + PAN + '~' + cCmbProofType.GetValue() + '~' + PAN + '~' + ctxtNumber.GetText() + '~' + ctxtIssuePlace.GetText() + '~' + validuntildt + '~' + issuedate + '~' + ccmbVerify.GetText());
                    else
                        grid.PerformCallback('saveold~' + type + '~' + PAN + '~' + cCmbProofType.GetValue() + '~' + PAN + '~' + ctxtNumber.GetText() + '~' + ctxtIssuePlace.GetText() + '~' + validuntildt + '~' + issuedate + '~' + ccmbVerify.GetText() + '~' + GetObjectID('hiddenedit').value);
                }
            }

            else if (type == "Service Tax Regn No") {

                //var PAN = document.getElementById('ASPxPageControl1_ASPxPageControl2_Popup_StatutoryRegistration_txtNumber_I').value;
                var PAN = document.getElementById('txtNumber_I').value;
                var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{3})(\d{3})$/;
                var code = /([C,P,H,F,A,T,B,L,J,G])/;
                var code_chk = PAN.substring(3, 4);
                if (PAN.search(panPat) == -1) {
                    jAlert("Please enter valid service Tax Regn No");
                    //Obj.focus();
                    return false;
                }
                if (code.test(code_chk) == false) {
                    jAlert("Invaild service Tax Regn No or Use Capital Letter.");
                    return false;
                }
                else {
                    if (GetObjectID('hiddenedit').value == '')
                        grid.PerformCallback('savenew~' + type + '~' + PAN + '~' + cCmbProofType.GetValue() + '~' + PAN + '~' + ctxtNumber.GetText() + '~' + ctxtIssuePlace.GetText() + '~' + validuntildt + '~' + issuedate + '~' + ccmbVerify.GetText());
                    else
                        grid.PerformCallback('saveold~' + type + '~' + PAN + '~' + cCmbProofType.GetValue() + '~' + PAN + '~' + ctxtNumber.GetText() + '~' + ctxtIssuePlace.GetText() + '~' + validuntildt + '~' + issuedate + '~' + ccmbVerify.GetText() + '~' + GetObjectID('hiddenedit').value);
                }
            }

            else if (type == "Pan Card") {
                //var PAN = document.getElementById('txtNumber_I').value;
                //var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
                //var code = /([C,P,H,F,A,T,B,L,J,G])/;
                //var code_chk = PAN.substring(3, 4);
                //if (PAN.search(panPat) == -1) {
                //    alert("Please Enter Valid Pan No"); 
                //    return false;
                //}
                //if (code.test(code_chk) == false) {
                //    alert("Invaild PAN Card No or Use Capital Letter");
                //    return false;
                //}

                var chk = cchkPan.GetChecked();
                if (ctxtNumber.GetText() == '') {
                    jAlert('Pan No Required !!');
                    ctxtNumber.Focus();
                }
                else {
                    if (chk == true) {
                        if (cCmbProofNumber.GetText() == '') {
                            jAlert('Proof No Required');
                            cCmbProofNumber.Focus();
                        }
                        else if (ctxtIssuePlace.GetText() == '') {
                            jAlert('Place Required');
                            ctxtIssuePlace.Focus();
                        }
                        else {
                            if (GetObjectID('hiddenedit').value == '')

                                grid.PerformCallback('savenew~' + type + '~' + chk + '~' + cCmbProofType.GetValue() + '~' + cCmbProofNumber.GetText() + '~' + ctxtNumber.GetText() + '~' + ctxtIssuePlace.GetText() + '~' + validuntildt + '~' + issuedate + '~' + ccmbVerify.GetText());
                            else
                                grid.PerformCallback('saveold~' + type + '~' + chk + '~' + cCmbProofType.GetValue() + '~' + cCmbProofNumber.GetText() + '~' + ctxtNumber.GetText() + '~' + ctxtIssuePlace.GetText() + '~' + validuntildt + '~' + issuedate + '~' + ccmbVerify.GetText() + '~' + GetObjectID('hiddenedit').value);
                        }
                    }

                    else {
                        var PAN = document.getElementById('txtNumber_I').value;
                        var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
                        var code = /([C,P,H,F,A,T,B,L,J,G])/;
                        var code_chk = PAN.substring(3, 4);
                        if (PAN.search(panPat) == -1) {
                            jAlert("Please Enter Valid Pan No");
                            return false;
                        }
                        if (code.test(code_chk) == false) {
                            jAlert("Invaild PAN Card No or Use Capital Letter");
                            return false;
                        }

                        if (GetObjectID('hiddenedit').value == '')
                            grid.PerformCallback('savenew~' + type + '~' + chk + '~' + ctxtNumber.GetText() + '~' + ccmbVerify.GetText());
                        else
                            grid.PerformCallback('saveold~' + type + '~' + chk + '~' + ctxtNumber.GetText() + '~' + ccmbVerify.GetText() + '~' + GetObjectID('hiddenedit').value);
                    }
                }

            }
            else if ((type == "VoterId") || (type == "SEBI Registration") || (type == "MAPIN CODE")) {
                if (ctxtNumber.GetText() == '') {
                    jAlert('Number Required');
                    ctxtNumber.Focus();
                }
                else if (ctxtIssuePlace.GetText() == '') {
                    jAlert('Place Required');
                    ctxtIssuePlace.Focus();
                }

                else {
                    if (GetObjectID('hiddenedit').value == '')
                        grid.PerformCallback('savenew~' + type + '~' + ctxtNumber.GetText() + '~' + ctxtIssuePlace.GetText() + '~' + issuedate + '~' + ccmbVerify.GetText());
                    else
                        grid.PerformCallback('saveold~' + type + '~' + ctxtNumber.GetText() + '~' + ctxtIssuePlace.GetText() + '~' + issuedate + '~' + ccmbVerify.GetText() + '~' + GetObjectID('hiddenedit').value);
                }
            }



            else if ((type == "MOA")) {
                if (ctxtNumber.GetText() == '') {
                    jAlert('MOA No Required');
                    ctxtNumber.Focus();
                }
                else if (ctxtIssuePlace.GetText() == '') {
                    jAlert('Place Required ');
                    ctxtIssuePlace.Focus();
                }
                else if (ctxtRegAuthName.GetText() == '') {
                    jAlert('Reg. Auth Name Required ');
                    ctxtRegAuthName.Focus();
                }
                else {
                    if (GetObjectID('hiddenedit').value == '')
                        grid.PerformCallback('savenew~' + type + '~' + ctxtNumber.GetText() + '~' + ctxtIssuePlace.GetText() + '~' + validuntildt + '~' + issuedate + '~' + ctxtRegAuthName.GetText());
                    else
                        grid.PerformCallback('saveold~' + type + '~' + ctxtNumber.GetText() + '~' + ctxtIssuePlace.GetText() + '~' + validuntildt + '~' + issuedate + '~' + ctxtRegAuthName.GetText() + '~' + GetObjectID('hiddenedit').value);
                }
            }

            else if (type == "Other") {
                if (ctxtRegAuthName.GetText() == '') {
                    jAlert('Reg. Auth Name Required ');
                    ctxtRegAuthName.Focus();
                }
                else if (issuedate == null) {
                    jAlert('Date field cannot be blanked ');
                    cdtIssue.Focus();
                    cdtIssue.SetDate(new Date());


                }
                else {
                    if (GetObjectID('hiddenedit').value == '')
                        grid.PerformCallback('savenew~' + type + '~' + ctxtNumber.GetText() + '~' + ctxtIssuePlace.GetText() + '~' + validuntildt + '~' + issuedate + '~' + ctxtRegAuthName.GetText());
                    else
                        grid.PerformCallback('saveold~' + type + '~' + ctxtNumber.GetText() + '~' + ctxtIssuePlace.GetText() + '~' + validuntildt + '~' + issuedate + '~' + ctxtRegAuthName.GetText() + '~' + GetObjectID('hiddenedit').value);
                }
            }

            else if (type == "CIN") {
                if (ctxtNumber.GetText() == '') {
                    jAlert('CIN No Required ');
                    ctxtNumber.Focus();
                }
                else if (ctxtIssuePlace.GetText() == '') {
                    jAlert('Place Required !!');
                    ctxtIssuePlace.Focus();
                }
                else if (ctxtRegAuthName.GetText() == '') {
                    jAlert('Reg. Auth Name Required !!');
                    ctxtRegAuthName.Focus();
                }
                else {
                    if (GetObjectID('hiddenedit').value == '')
                        grid.PerformCallback('savenew~' + type + '~' + ctxtNumber.GetText() + '~' + ctxtIssuePlace.GetText() + '~' + validuntildt + '~' + issuedate + '~' + ctxtRegAuthName.GetText() + '~' + ccmbVerify.GetText());
                    else
                        grid.PerformCallback('saveold~' + type + '~' + ctxtNumber.GetText() + '~' + ctxtIssuePlace.GetText() + '~' + validuntildt + '~' + issuedate + '~' + ctxtRegAuthName.GetText() + '~' + ccmbVerify.GetText() + '~' + GetObjectID('hiddenedit').value);
                }
            }
            else if (type == "Aadhar Card") {

                if (ctxtNumber.GetText() == '') {
                    jAlert('Aadhar Card No Required !!');
                    ctxtNumber.Focus();
                }
                else {
                    if (GetObjectID('hiddenedit').value == '')
                        grid.PerformCallback('savenew~' + type + '~' + ctxtNumber.GetText() + '~' + ccmbVerify.GetText());
                    else
                        grid.PerformCallback('saveold~' + type + '~' + ctxtNumber.GetText() + '~' + ccmbVerify.GetText() + '~' + GetObjectID('hiddenedit').value);
                }
            }
            else {
                if (ctxtNumber.GetText() == '') {
                    jAlert('Number Required !!');
                    ctxtNumber.Focus();
                }
                else if (ctxtIssuePlace.GetText() == '') {
                    jAlert('Place Required !!');
                    ctxtIssuePlace.Focus();
                }


                else {
                    if (GetObjectID('hiddenedit').value == '')
                        grid.PerformCallback('savenew~' + type + '~' + ctxtNumber.GetText() + '~' + ctxtIssuePlace.GetText() + '~' + validuntildt + '~' + issuedate + '~' + ccmbVerify.GetText());
                    else
                        grid.PerformCallback('saveold~' + type + '~' + ctxtNumber.GetText() + '~' + ctxtIssuePlace.GetText() + '~' + validuntildt + '~' + issuedate + '~' + ccmbVerify.GetText() + '~' + GetObjectID('hiddenedit').value);
                }
            }
            GetObjectID('hiddenedit').value = '';

        }
        //    function DuplicatePanInsertionConfirmation()
        //    {
        //       var i=confirm("Are you sure to enter this duplicate PAN");
        //       if(i==true)
        //       {
        //        j=confirm("Are you sure to enter this duplicate PAN");
        //          if(j==true)
        //           {
        //            k=confirm("Are you sure to enter this duplicate PAN");
        //            if(k==true)
        //            {
        //              return true;
        //            }
        //       }
        //       return false;
        //    }   
        //    function BannedPanInsertionConfirmation()
        //    {
        //       var i=confirm("Are you sure to enter this banned PAN");
        //       if(i==true)
        //       {
        //        j=confirm("Are you sure to enter this banned PAN");
        //          if(j==true)
        //           {
        //            k=confirm("Are you sure to enter this banned PAN");
        //            if(k==true)
        //            {
        //              return true;
        //            }
        //       }
        //       return false;
        //    }    

        function grid_EndCallBack() {
            if (grid.cpinsert != null) {
                // alert(grid.cpinsert);
                if (grid.cpinsert == 'success') {
                    jAlert('Saved Successfully');
                    cPopup_StatutoryRegistration.Hide();
                    grid.PerformCallback('onlybind~1');
                    grid.cpinsert = null
                }

                else {

                    if (grid.cppanMsg != null) {
                        var l = false;
                        // alert(grid.cppanMsg);
                        var i = confirm(grid.cppanMsg);
                        if (i == false) {
                            var j = confirm("Are you sure enter this PAN ?");
                            if (j == true) {
                                var k = confirm("Are you sure enter this PAN ?");
                                if (k == true) {
                                    l = confirm("Are you sure enter this PAN ?");
                                }
                            }
                            if (l == true) {
                                //pbm
                                if (grid.cppanDuplicate != null) {
                                    document.getElementById("hdnDuplicatePAN").value = 'y';
                                }
                                else if (grid.cppanBanned != null) {
                                    document.getElementById("hdnBannedPAN").value = 'y';

                                }

                                btnSave_Registration();
                            }
                            else {
                                document.getElementById("hdnDuplicatePAN").value = 'n';
                                document.getElementById("hdnBannedPAN").value = 'n';
                                cPopup_StatutoryRegistration.Hide();
                                grid.PerformCallback('onlybind~1');

                            }
                        }
                        else {
                            cPopup_StatutoryRegistration.Hide();
                            grid.PerformCallback('onlybind~1');
                        }
                    }

                    else {
                        jAlert('Error on Insertion/n Please Try again!!');
                    }

                }

            }
            if (grid.cpupdate != null) {
                // alert(grid.cpupdate);
                if (grid.cpupdate == 'success') {
                    jAlert('Update Successfully');
                    cPopup_StatutoryRegistration.Hide();
                    grid.PerformCallback('onlybind~1');
                    grid.cpupdate = null

                }

                else {
                    if (grid.cppanMsg != null) {
                        // alert(grid.cppanMsg);
                        var l = false;
                        var i = confirm(grid.cppanMsg);
                        if (i == false) {
                            var j = confirm("Are you sure enter this PAN ?");
                            if (j == true) {
                                var k = confirm("Are you sure enter this PAN ?");
                                if (k == true) {
                                    l = confirm("Are you sure enter this PAN ?");
                                }
                            }
                            if (l == true) {
                                if (grid.cppanDuplicate != null) {
                                    document.getElementById("hdnDuplicatePAN").value = 'y';
                                }
                                else if (grid.cppanBanned != null) {
                                    document.getElementById("hdnBannedPAN").value = 'y';
                                }

                                btnSave_Registration();
                            }
                            else {
                                document.getElementById("hdnDuplicatePAN").value = 'n';
                                document.getElementById("hdnBannedPAN").value = 'n';
                                cPopup_StatutoryRegistration.Hide();
                                grid.PerformCallback('onlybind~1');
                            }
                        }
                        else {
                            cPopup_StatutoryRegistration.Hide();
                            grid.PerformCallback('onlybind~1');
                        }

                    }
                    else {
                        jAlert('Error on Updation/n Please Try again!!');
                    }

                }

            }
            if (grid.cpdelete != null) {
                if (grid.cpdelete == 'deleteok') {
                    jAlert('Deleted Successfully');
                    grid.PerformCallback('onlybind~1');
                    grid.cpdelete = null
                }
                else
                    jAlert('Error on Deletion/n Please Try again!!');

            }
            if (grid.cpalreadyexist != null) {
                jAlert(grid.cpalreadyexist.split('~')[1] + ' Type of Document Already Exist For You');
                grid.cpalreadyexist = null
            }
            if (grid.cpedit != null) {

                ccmbSR.SetText(grid.cpedit.split('~')[0]);
                var verified = grid.cpedit.split('~')[8];

                if (ccmbSR.GetText() == 'Pan Card') {
                    cchkPan.SetEnabled(false);
                    if (grid.cpedit.split('~')[3] == 'PAN_EXEMPT') {

                        cchkPan.SetChecked(true);

                        ctxtRegAuthName.SetText('Not Applicable');
                        ctxtNumber.SetEnabled(false);
                        ccmbSR.SetEnabled(false);
                        cCmbProofType.SetEnabled(true);
                        cCmbProofNumber.SetEnabled(true);
                        ctxtIssuePlace.SetEnabled(true);
                        cdtValid.SetEnabled(true);
                        cdtIssue.SetEnabled(true);
                        ccmbVerify.SetEnabled(true);
                        ctxtRegAuthName.SetEnabled(false)
                        cCmbProofType.SetValue(grid.cpedit.split('~')[1]);
                        cCmbProofNumber.SetText(grid.cpedit.split('~')[2]);
                        ctxtNumber.SetText(grid.cpedit.split('~')[3]);
                        ctxtIssuePlace.SetText(grid.cpedit.split('~')[4]);
                        cdtValid.SetDate(ChangeDateFormat_SetCalenderValue(grid.cpedit.split('~')[5]));
                        //cdtValid.SetDate(ChangeDateFormat_CalToDB(grid.cpedit.split('~')[5]));

                        //alert(grid.cpedit.split('~')[5]);
                        //cdtValid.SetDate(grid.cpedit.split('~')[5]);
                        cdtIssue.SetDate(ChangeDateFormat_SetCalenderValue(grid.cpedit.split('~')[6]));
                        //cdtIssue.SetDate(ChangeDateFormat_CalToDB(grid.cpedit.split('~')[6]));
                        //cdtIssue.SetDate(grid.cpedit.split('~')[6]);
                        if (verified == '1') {

                            ccmbVerify.SetText('Verified');
                        }
                        else {
                            ccmbVerify.SetText('Not Verified');
                        }
                        //ccmbVerify.SetText(grid.cpedit.split('~')[8]);

                    }
                    else {
                        cchkPan.SetChecked(false);
                        ctxtIssuePlace.SetText('Not Applicable');
                        ctxtRegAuthName.SetText('Not Applicable');
                        cCmbProofNumber.SetText('Not Applicable');
                        ctxtNumber.SetEnabled(true);
                        ccmbSR.SetEnabled(false);
                        cCmbProofType.SetEnabled(false);
                        cCmbProofNumber.SetEnabled(false);
                        ctxtIssuePlace.SetEnabled(false);
                        cdtValid.SetEnabled(false);
                        cdtIssue.SetEnabled(false);
                        ccmbVerify.SetEnabled(true);
                        ctxtRegAuthName.SetEnabled(false)
                        cCmbProofType.SetValue(1);
                        ctxtNumber.SetText(grid.cpedit.split('~')[3]);
                        cdtValid.SetDate(new Date());
                        cdtIssue.SetDate(new Date());
                        if (verified == '1') {

                            ccmbVerify.SetText('Verified');
                        }
                        else {
                            ccmbVerify.SetText('Not Verified');
                        }
                        //ccmbVerify.SetText(grid.cpedit.split('~')[8]);

                    }


                }

                else if (ccmbSR.GetText() == "VoterId" || ccmbSR.GetText() == "MAPIN CODE" || ccmbSR.GetText() == "SEBI Registration") {
                    ctxtNumber.SetText(grid.cpedit.split('~')[3]);
                    ctxtIssuePlace.SetText(grid.cpedit.split('~')[4]);
                    cdtValid.SetDate(ChangeDateFormat_SetCalenderValue(grid.cpedit.split('~')[5]));
                    cdtIssue.SetDate(ChangeDateFormat_SetCalenderValue(grid.cpedit.split('~')[6]));
                    ctxtRegAuthName.SetText(grid.cpedit.split('~')[7]);
                    if (verified == '1') {

                        ccmbVerify.SetText('Verified');
                    }
                    else {
                        ccmbVerify.SetText('Not Verified');
                    }
                    //ccmbVerify.SetText(grid.cpedit.split('~')[8]);
                    cCmbProofType.SetValue(1);
                    ctxtRegAuthName.SetText('Not Applicable');
                    cCmbProofNumber.SetText('Not Applicable');
                    ctxtNumber.SetEnabled(true);
                    ccmbSR.SetEnabled(false);
                    cCmbProofType.SetEnabled(false);
                    cCmbProofNumber.SetEnabled(false);
                    ctxtIssuePlace.SetEnabled(true);
                    cdtValid.SetEnabled(false);
                    cdtIssue.SetEnabled(true);
                    ccmbVerify.SetEnabled(true);
                    ctxtRegAuthName.SetEnabled(false);
                    HideShow('PanCheck', 'H');

                }

                else if ((ccmbSR.GetText() == "MOA") || (ccmbSR.GetText() == "Other")) {
                    ctxtNumber.SetText(grid.cpedit.split('~')[3]);
                    ctxtIssuePlace.SetText(grid.cpedit.split('~')[4]);
                    cdtValid.SetDate(ChangeDateFormat_SetCalenderValue(grid.cpedit.split('~')[5]));
                    cdtIssue.SetDate(ChangeDateFormat_SetCalenderValue(grid.cpedit.split('~')[6]));
                    ctxtRegAuthName.SetText(grid.cpedit.split('~')[7]);
                    cCmbProofType.SetValue(1);

                    cCmbProofNumber.SetText('Not Applicable');
                    ctxtNumber.SetEnabled(true);
                    ccmbSR.SetEnabled(false);
                    cCmbProofType.SetEnabled(false);
                    cCmbProofNumber.SetEnabled(false);
                    ctxtIssuePlace.SetEnabled(true);
                    cdtValid.SetEnabled(false);
                    cdtIssue.SetEnabled(true);
                    ccmbVerify.SetEnabled(false);
                    ctxtRegAuthName.SetEnabled(true);
                    HideShow('PanCheck', 'H');
                }
                else if (ccmbSR.GetText() == "CIN") {
                    ctxtNumber.SetText(grid.cpedit.split('~')[3]);
                    ctxtIssuePlace.SetText(grid.cpedit.split('~')[4]);
                    cdtValid.SetDate(ChangeDateFormat_SetCalenderValue(grid.cpedit.split('~')[5]));
                    cdtIssue.SetDate(ChangeDateFormat_SetCalenderValue(grid.cpedit.split('~')[6]));
                    ctxtRegAuthName.SetText(grid.cpedit.split('~')[7]);
                    if (verified == '1') {

                        ccmbVerify.SetText('Verified');
                    }
                    else {
                        ccmbVerify.SetText('Not Verified');
                    }
                    //ccmbVerify.SetText(grid.cpedit.split('~')[8]);
                    cCmbProofType.SetValue(1);
                    cCmbProofNumber.SetText('Not Applicable');
                    ctxtNumber.SetEnabled(true);
                    ccmbSR.SetEnabled(false);
                    cCmbProofType.SetEnabled(false);
                    cCmbProofNumber.SetEnabled(false);
                    ctxtIssuePlace.SetEnabled(true);
                    cdtValid.SetEnabled(false);
                    cdtIssue.SetEnabled(true);
                    ccmbVerify.SetEnabled(true);
                    ctxtRegAuthName.SetEnabled(false);
                    HideShow('PanCheck', 'H');

                }
                else if (ccmbSR.GetText() == "Aadhar Card") {


                    ctxtIssuePlace.SetText('Not Applicable');
                    ctxtRegAuthName.SetText('Not Applicable');
                    cCmbProofNumber.SetText('Not Applicable');
                    ctxtNumber.SetEnabled(true);
                    ccmbSR.SetEnabled(false);
                    cCmbProofType.SetEnabled(false);
                    cCmbProofNumber.SetEnabled(false);
                    ctxtIssuePlace.SetEnabled(false);
                    cdtValid.SetEnabled(false);
                    cdtIssue.SetEnabled(false);
                    ccmbVerify.SetEnabled(true);
                    ctxtRegAuthName.SetEnabled(false)
                    cCmbProofType.SetValue(1);
                    ctxtNumber.SetText(grid.cpedit.split('~')[3]);
                    cdtValid.SetDate(new Date());
                    cdtIssue.SetDate(new Date());
                    if (verified == '1') {

                        ccmbVerify.SetText('Verified');
                    }
                    else {
                        ccmbVerify.SetText('Not Verified');
                    }
                    //ccmbVerify.SetText(grid.cpedit.split('~')[8]);
                    HideShow('PanCheck', 'H');

                }

                    //.... Code Added By Sam on 17102016....................................
                else if ((ccmbSR.GetText() == "VAT Regn No") || (ccmbSR.GetText() == "CST Regn No") || (ccmbSR.GetText() == "ECC Regn No") || (ccmbSR.GetText() == "Service Tax Regn No") || (ccmbSR.GetText() == "SGST") || (ccmbSR.GetText() == "CGST") || (ccmbSR.GetText() == "IGST")) {

                    ctxtIssuePlace.SetText('Not Applicable');
                    ctxtRegAuthName.SetText('Not Applicable');
                    cCmbProofNumber.SetText('Not Applicable');
                    ctxtNumber.SetEnabled(true);
                    ccmbSR.SetEnabled(false);
                    cCmbProofType.SetEnabled(false);
                    cCmbProofNumber.SetEnabled(false);
                    ctxtIssuePlace.SetEnabled(false);
                    cdtValid.SetEnabled(false);
                    cdtIssue.SetEnabled(false);
                    ccmbVerify.SetEnabled(true);
                    ctxtRegAuthName.SetEnabled(false)
                    cCmbProofType.SetValue(1);
                    ctxtNumber.SetText(grid.cpedit.split('~')[3]);
                    cdtValid.SetDate(new Date());
                    cdtIssue.SetDate(new Date());

                    if (verified == '1') {

                        ccmbVerify.SetText('Verified');
                    }
                    else {
                        ccmbVerify.SetText('Not Verified');
                    }
                    //ccmbVerify.SetText(grid.cpedit.split('~')[8]);
                    HideShow('PanCheck', 'H');

                }
                    //.... Code Above Added By Sam on 17102016....................................
                else {
                    ctxtNumber.SetText(grid.cpedit.split('~')[3]);
                    ctxtIssuePlace.SetText(grid.cpedit.split('~')[4]);
                    cdtValid.SetDate(ChangeDateFormat_SetCalenderValue(grid.cpedit.split('~')[5]));
                    cdtIssue.SetDate(ChangeDateFormat_SetCalenderValue(grid.cpedit.split('~')[6]));
                    if (verified == '1') {

                        ccmbVerify.SetText('Verified');
                    }
                    else {
                        ccmbVerify.SetText('Not Verified');
                    }
                    //ccmbVerify.SetText(grid.cpedit.split('~')[8]);
                    cCmbProofType.SetValue(1);
                    ctxtRegAuthName.SetText('Not Applicable');
                    cCmbProofNumber.SetText('Not Applicable');
                    ctxtNumber.SetEnabled(true);
                    ccmbSR.SetEnabled(false);
                    cCmbProofType.SetEnabled(false);
                    cCmbProofNumber.SetEnabled(false);
                    ctxtIssuePlace.SetEnabled(true);
                    cdtValid.SetEnabled(true);
                    cdtIssue.SetEnabled(true);
                    ccmbVerify.SetEnabled(true);
                    ctxtRegAuthName.SetEnabled(false);
                    HideShow('PanCheck', 'H');
                }
                GetObjectID('hiddenedit').value = grid.cpedit.split('~')[9];
                cbtnSave_Registration.SetText('Update');
                cPopup_StatutoryRegistration.Show();
                grid.cpedit = null
            }
        }


        function disp_prompt(name) {
            if (name == "tab0") {
                //alert(name);
                document.location.href = "HRrecruitmentagent_general.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "HRrecruitmentagent_ContactPerson.aspx";
            }
            else if (name == "tab2") {
                //alert(name);
                document.location.href = "HRrecruitmentagent_Correspondence.aspx";
            }
            else if (name == "tab3") {
                //alert(name);
                document.location.href = "HRrecruitmentagent_BankDetails.aspx";
            }
            else if (name == "tab4") {
                document.location.href = "HRrecruitmentagent_Document.aspx";
            }
            else if (name == "tab5") {
                //alert(name);
                //document.location.href = "HRrecruitmentagent_Registration.aspx";
            }
            else if (name == "tab6") {
                document.location.href = "HRrecruitmentagent_GroupMember.aspx";

            }
            else if (name == "tab7") {
                document.location.href = "Vendors_Tds.aspx";

            }

        }
        function change() {
            compSegment.PerformCallback();
        }
        function CallAjaxForSubBroker(objid, objfunc, objevent) {
            ajax_showOptions(objid, objfunc, objevent);
        }
        function CallAjaxForDealer(objid, objfunc, objevent) {
            ajax_showOptions(objid, objfunc, objevent);
        }
        function DrpValueSelect(objval) {
            if (objval != '') {

                var obj = objval.split('~');
                if (obj[0] == 'Show') {
                    compCompany.SetValue(obj[1]);
                }
                else {
                    compSegment.SetValue(obj[0]);
                    compSegment.SetEnabled(false);
                    document.getElementById(obj[1]).value = obj[2];
                    if (obj[4] != '')
                        document.getElementById(obj[3]).value = obj[4];
                    cComboSwapMapinSebi.SetValue(obj[5]);
                }
            }
        }
        function Emailcheck(obj, obj2) {
            if (obj2 != 'N') {
                dt = confirm('This entity has been banned by SEBI see the details: ' + obj2 + ' !!!.\n\nDo You Want To Delete???');
                if (dt) {
                    obj = 'Delete';
                    grid.PerformCallback(obj);
                }
                else {
                    if (obj != 'b') {
                        doIt = confirm(obj + ' has same PAN number!!!.\n\n Do You Want To Delete???');
                        if (doIt) {
                            obj = 'Delete';
                            grid.PerformCallback(obj);
                        }
                        else {
                            IScON = confirm('Warning!!.\n\n Are You Sure You Want Accept This PAN Number??.\n\nClick OK For Accept,Otherwise Click Cancel');
                            if (IScON) {
                                INT = confirm('Warning!!.\n\n Are You Sure You Want Accept This PAN Number??.\n\nClick OK For Accept,Otherwise Click Cancel');
                                if (INT) {
                                    INR = confirm('Warning!!.\n\n Are You Sure You Want Accept This PAN Number??.\n\nClick OK For Accept,Otherwise Click Cancel');
                                    if (INR) {
                                        jAlert('Your PAN Number Has Been Accepted.')
                                    }
                                    else {
                                        obj = 'Delete';
                                        grid.PerformCallback(obj);
                                    }

                                }
                                else {
                                    obj = 'Delete';
                                    grid.PerformCallback(obj);
                                }

                            }
                            else {
                                obj = 'Delete';
                                grid.PerformCallback(obj);
                            }

                        }
                    }
                    else {
                        BNRT = confirm('Warning!!.\n\n Are You Sure You Want Accept This PAN Number??.\n\nClick OK For Accept,Otherwise Click Cancel');
                        if (BNRT) {
                            SGH = confirm('Warning!!.\n\n Are You Sure You Want Accept This PAN Number??.\n\nClick OK For Accept,Otherwise Click Cancel');
                            if (SGH) {
                                BNT = confirm('Warning!!.\n\n Are You Sure You Want Accept This PAN Number??.\n\nClick OK For Accept,Otherwise Click Cancel');
                                if (BNT) {
                                    jAlert('Your PAN Number Has Been Accepted.')
                                }
                                else {
                                    obj = 'Delete';
                                    grid.PerformCallback(obj);
                                }

                            }
                            else {
                                obj = 'Delete';
                                grid.PerformCallback(obj);
                            }

                        }
                        else {
                            obj = 'Delete';
                            grid.PerformCallback(obj);
                        }
                    }


                }





            }
            else if (obj != 'b') {
                doIt = confirm(obj + ' Has Same PAN Number!!!.\n\nDo You Want To Delete???');
                if (doIt) {
                    obj = 'Delete';
                    grid.PerformCallback(obj);
                }
                else {
                    ISON = confirm('Warning!!.\n\n Are You Sure You Want Accept This PAN Number??.\n\nClick OK For Accept,Otherwise Click Cancel');
                    if (ISON) {
                        IJK = confirm('Warning!!.\n\n Are You Sure You Want Accept This PAN Number??.\n\nClick OK For Accept,Otherwise Click Cancel');
                        if (IJK) {
                            MKJ = confirm('Warning!!.\n\n Are You Sure You Want Accept This PAN Number??.\n\nClick OK For Accept,Otherwise Click Cancel');
                            if (MKJ) {
                                jAlert('Your PAN Number Has Been Accepted.')
                            }
                            else {
                                obj = 'Delete';
                                grid.PerformCallback(obj);

                            }
                        }
                        else {
                            obj = 'Delete';
                            grid.PerformCallback(obj);

                        }
                    }
                    else {
                        obj = 'Delete';
                        grid.PerformCallback(obj);

                    }
                }

            }

        }

        //     FieldName='ASPxPageControl1_gridMembership_DXTDGScol8'




        FieldName = 'ASPxPageControl1_gridMembership_DXTDGScol8';




    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input type="hidden" id="hfSelectedType">
    <div class="panel-heading">
        <div class="panel-title">
            <%-- <h3>Contact Registration List</h3>--%>
            <h3>
                <asp:Label ID="lblHeadTitle" runat="server"></asp:Label>
            </h3>
             <div class="crossBtn"><a href="HRrecruitmentagent.aspx"><i class="fa fa-times"></i></a></div>
        </div>

    </div>
    <div class="form_main">
        <table width="100%">
             <tr>
                <td style="text-align: center">
                    <asp:Label ID="lblHeader" runat="server" Font-Bold="True" Font-Size="15px" ForeColor="Navy"
                        Width="819px" Height="18px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="EHEADER" style="text-align: center">
                    <asp:Label ID="lblName" runat="server" Font-Bold="True"></asp:Label>
                </td>
            </tr>
        </table>
        <table class="TableMain100">
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="5" ClientInstanceName="page"
                        Font-Size="12px">
                        <TabPages>
                            <dxe:TabPage Name="General" Text="General">

                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                             <dxe:TabPage Text="Contact Person" Name="ContactPreson">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="CorresPondence" Text="Correspondence">

                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Bank" Text="Bank Details">

                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Documents" Text="Documents">

                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Registration" Text="Registration">

                                <ContentCollection>
                                    <dxe:ContentControl runat="server">

                                        <dxe:ASPxPageControl ID="ASPxPageControl2" runat="server" ActiveTabIndex="0" Width="100%"
                                            ClientInstanceName="page">
                                            <TabPages>
                                                <dxe:TabPage Name="Statutory Registrations" Text="Statutory Registrations">

                                                    <ContentCollection>
                                                        <dxe:ContentControl runat="server">
                                                            <div style="float: left;">
                                                                <% if (rights.CanAdd)
                                                                   { %>
                                                                <a href="javascript:void(0);" class="btn btn-primary" onclick="fn_PopOpen();"><span>Add New</span> </a>
                                                                <%} %>
                                                            </div>
                                                            <dxe:ASPxGridView ID="gridRegisStatutory" KeyFieldName="crg_id" ClientInstanceName="grid"
                                                                runat="server" Width="100%" AutoGenerateColumns="False" OnInitNewRow="gridRegisStatutory_InitNewRow"
                                                                OnRowValidating="gridRegisStatutory_RowValidating" OnCustomCallback="gridRegisStatutory_CustomCallback">
                                                                <%--<Settings ShowFilterRow="true"  ShowGroupPanel="true"/>--%>
                                                                <Columns>
                                                                    <dxe:GridViewDataTextColumn FieldName="crg_id" ReadOnly="True" VisibleIndex="0"
                                                                        Visible="False">
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="crg_cntId" VisibleIndex="0" Visible="False">
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataComboBoxColumn Caption="Type" FieldName="crg_type" VisibleIndex="0">
                                                                        <PropertiesComboBox EnableSynchronization="False" EnableIncrementalFiltering="True"
                                                                            ValueType="System.String">
                                                                            <Items>



                                                                                <dxe:ListEditItem Text="PAN Card" Value="Pan Card"></dxe:ListEditItem>



                                                                                <dxe:ListEditItem Text="Passport" Value="Passport"></dxe:ListEditItem>



                                                                                <dxe:ListEditItem Text="Ration Card" Value="RationCard"></dxe:ListEditItem>



                                                                                <dxe:ListEditItem Text="Driving License" Value="DrivingLicense"></dxe:ListEditItem>



                                                                                <dxe:ListEditItem Text="Voter Id" Value="VoterId"></dxe:ListEditItem>



                                                                                <dxe:ListEditItem Text="Social Security Number" Value="SSNumber"></dxe:ListEditItem>



                                                                                <dxe:ListEditItem Text="SEBI Registration" Value="SEBIRegn"></dxe:ListEditItem>



                                                                                <dxe:ListEditItem Text="MAPIN CODE" Value="MpCode"></dxe:ListEditItem>



                                                                                <dxe:ListEditItem Text="RBI Ref. No" Value="RbiNo"></dxe:ListEditItem>



                                                                                <dxe:ListEditItem Text="MOA" Value="MOA"></dxe:ListEditItem>



                                                                                <dxe:ListEditItem Text="FMC Registration" Value="FmCRegis"></dxe:ListEditItem>



                                                                                <dxe:ListEditItem Text="CIN" Value="CIN"></dxe:ListEditItem>



                                                                                <dxe:ListEditItem Text="Other" Value="Other"></dxe:ListEditItem>



                                                                                <dxe:ListEditItem Text="Aadhar Card" Value="AdharId"></dxe:ListEditItem>



                                                                            </Items>



                                                                        </PropertiesComboBox>
                                                                        <EditFormSettings Caption="Type" Visible="True" VisibleIndex="0" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="crg_Number" VisibleIndex="1" Caption="Number">
                                                                        <PropertiesTextEdit MaxLength="50"></PropertiesTextEdit>
                                                                        <EditFormSettings Caption="Number" Visible="True" VisibleIndex="1" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <PropertiesTextEdit MaxLength="50"></PropertiesTextEdit>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataComboBoxColumn Caption="Proof Type" FieldName="crg_PanExmptProofType"
                                                                        VisibleIndex="8" Visible="false">
                                                                        <PropertiesComboBox EnableSynchronization="False" EnableIncrementalFiltering="true"
                                                                            ValueType="System.String">
                                                                            <Items>



                                                                                <dxe:ListEditItem Text="Mapin ID" Value="1"></dxe:ListEditItem>



                                                                                <dxe:ListEditItem Text="Passport" Value="2"></dxe:ListEditItem>



                                                                                <dxe:ListEditItem Text="Voter ID" Value="3"></dxe:ListEditItem>



                                                                                <dxe:ListEditItem Text="Driving License" Value="4"></dxe:ListEditItem>



                                                                                <dxe:ListEditItem Text="Ration Card" Value="5"></dxe:ListEditItem>



                                                                                <dxe:ListEditItem Text="Registration Details" Value="6"></dxe:ListEditItem>



                                                                                <dxe:ListEditItem Text="UID (Aadhar ID)" Value="7"></dxe:ListEditItem>



                                                                            </Items>



                                                                        </PropertiesComboBox>
                                                                        <EditFormSettings Caption="Proof Type" Visible="True" VisibleIndex="2" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="crg_registrationAuthority" VisibleIndex="6"
                                                                        Caption="Registration Authority Name" Visible="False">
                                                                        <EditFormSettings Caption="Registration Authority Name/Remarks" Visible="True" VisibleIndex="6" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="crg_PanExmptProofNumber" VisibleIndex="6"
                                                                        Caption="Proof Number" Visible="False">
                                                                        <EditFormSettings Caption="Proof Number" Visible="True" VisibleIndex="3" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="crg_place" VisibleIndex="2" Caption="Place of Issue">
                                                                        <PropertiesTextEdit MaxLength="50"></PropertiesTextEdit>
                                                                        <EditFormSettings Caption="Place of Issue" Visible="True" VisibleIndex="3" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataDateColumn Caption="Date of Issue" FieldName="crg_Date" Visible="true">
                                                                        <PropertiesDateEdit EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" DisplayFormatString="dd MMM yyyy">
                                                                        </PropertiesDateEdit>
                                                                        <EditFormSettings Caption="Date of Issue" Visible="True" VisibleIndex="4" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataDateColumn>
                                                                    <%--<dxe:GridViewDataTextColumn FieldName="crg_Date1" Caption="Date of Issue" VisibleIndex="3">
                                                                        <EditFormSettings Caption="Date of Issue" Visible="false" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>--%>
                                                                    <dxe:GridViewDataDateColumn Caption="Valid Until" FieldName="crg_validDate" Visible="true">
                                                                        <PropertiesDateEdit EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" DisplayFormatString="dd MMM yyyy">
                                                                        </PropertiesDateEdit>
                                                                        <EditFormSettings Caption="Valid Until" Visible="True" VisibleIndex="5" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataDateColumn>
                                                                   <%-- <dxe:GridViewDataTextColumn FieldName="crg_validDate1" VisibleIndex="4" Caption="Valid Until">
                                                                        <EditFormSettings Caption="Valid Until" Visible="false" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>--%>
                                                                    <dxe:GridViewDataComboBoxColumn Caption="Verify" FieldName="crg_verify" Visible="false">
                                                                        <PropertiesComboBox EnableSynchronization="False" EnableIncrementalFiltering="True"
                                                                            ValueType="System.String">
                                                                            <Items>



                                                                                <dxe:ListEditItem Text="Verified" Value="Verified"></dxe:ListEditItem>



                                                                                <dxe:ListEditItem Text="Not Verified" Value="Not Verified"></dxe:ListEditItem>



                                                                            </Items>



                                                                        </PropertiesComboBox>
                                                                        <EditFormSettings Caption="Verify" Visible="True" VisibleIndex="7" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>
                                                                    <dxe:GridViewDataDateColumn FieldName="CreateDate" VisibleIndex="5" Visible="False">
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataDateColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="CreateUser" VisibleIndex="5" Visible="False">
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataDateColumn FieldName="LastModifyDate" VisibleIndex="5" Visible="False">
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataDateColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="LastModifyUser" VisibleIndex="5" Visible="False">
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn ReadOnly="True" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="6%" Visible="true">
                                                                        <DataItemTemplate>
                                                                            <% if (rights.CanEdit)
                                                                               {     %>
                                                                            <a href="javascript:void(0);" onclick="fn_PopEditOpen('<%# Container.KeyValue %>')" class="pad">
                                                                                <img src="../../../assests/images/Edit.png" /></a>
                                                                            <% } %>
                                                                            <% if (rights.CanDelete)
                                                                               { %>
                                                                            <a href="javascript:void(0);" onclick="fn_Delete('<%# Container.KeyValue %>')">
                                                                                <img src="../../../assests/images/Delete.png" /></a>
                                                                            <% } %>
                                                                        </DataItemTemplate>
                                                                        <EditFormSettings Visible="False" />
                                                                        <CellStyle HorizontalAlign="Left">
                                                                        </CellStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <HeaderTemplate>
                                                                            Modify/ Delete
                                                                            <%-- <% if (rights.CanAdd)
                                                                                     { %>--%>
                                                                            <%--<a href="javascript:void(0);" onclick="fn_PopOpen();"><span>Add New</span> </a>--%>
                                                                            <%-- <% } %>--%>
                                                                        </HeaderTemplate>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <%--<dxe:GridViewCommandColumn VisibleIndex="5">
                                                                      
                                                                        <DeleteButton Visible="True">
                                                                        </DeleteButton>
                                                                        <HeaderTemplate>
                                                                            <a href="javascript:void(0);" onclick="fn_PopOpen();"><span style="color: #000099;
                                                                                text-decoration: underline">Add New</span> </a>
                                                                        </HeaderTemplate>
                                                                    </dxe:GridViewCommandColumn>--%>
                                                                </Columns>
                                                                <StylesEditors>
                                                                    <ProgressBar Height="25px">
                                                                    </ProgressBar>
                                                                </StylesEditors>
                                                                <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="300px" PopupEditFormHorizontalAlign="Center"
                                                                    PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="450px"
                                                                    EditFormColumnCount="1" />
                                                                <Styles>
                                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                    </Header>
                                                                    <LoadingPanel ImageSpacing="10px">
                                                                    </LoadingPanel>
                                                                </Styles>
                                                                <SettingsSearchPanel Visible="True" />
                                                                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                                                <SettingsText PopupEditFormCaption="Add/Modify Registration" ConfirmDelete="Confirm delete?" />
                                                                <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                                                                    <FirstPageButton Visible="True">
                                                                    </FirstPageButton>
                                                                    <LastPageButton Visible="True">
                                                                    </LastPageButton>
                                                                </SettingsPager>
                                                                <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="true" />
                                                                <Templates>
                                                                    <TitlePanel>
                                                                        <div style="background-color: rgb(237,243,244); color: #000; margin: 8px 8px 0px 8px">
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td align="center" style="width: 50%">
                                                                                        <span class="Ecoheadtxt" style="color: Black">Statutory
                                                                                        Registrations</span>
                                                                                    </td>
                                                                                    <%-- <td align="right">
                                                                            <table width="200">
                                                                                <tr>
                                                                                    
                                                                                   
                                                                                    <td>
                                                                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" Text="ADD" ToolTip="Add New Data"   Height="18px" Width="88px" AutoPostBack="False">
                                                                                            <clientsideevents click="function(s, e) {grid.AddNewRow();}" />
                                                                                        </dxe:ASPxButton>
                                                                                    </td>
                                                                                                                        
                                                                                     
                                                                                  </tr>
                                                                              </table>
                                                                          </td>   --%>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </TitlePanel>
                                                                    <EditForm>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: 5%"></td>
                                                                                <td style="width: 90%">

                                                                                    <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors"></dxe:ASPxGridViewTemplateReplacement>

                                                                                    <div style="text-align: center; padding: 2px 2px 2px 2px">
                                                                                        <div class="dxbButton" style="display: inline-block; padding: 3px">
                                                                                            <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                                                                runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                        </div>
                                                                                        <div class="dxbButton" style="display: inline-block; padding: 3px">
                                                                                            <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                                                                runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                        </div>
                                                                                    </div>
                                                                                </td>
                                                                                <td style="width: 5%"></td>
                                                                            </tr>
                                                                        </table>
                                                                    </EditForm>
                                                                </Templates>
                                                                <ClientSideEvents EndCallback="function(s, e) {Emailcheck(s.cpHeight,s.cpWidth);}" />
                                                                <ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack();}" />
                                                            </dxe:ASPxGridView>
                                                            <dxe:ASPxPopupControl ID="Popup_StatutoryRegistration" runat="server" ClientInstanceName="cPopup_StatutoryRegistration"
                                                                HeaderText="Client Statutory Registration Details" PopupHorizontalAlign="WindowCenter"
                                                                PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" Modal="True">
                                                                <ContentCollection>
                                                                    <dxe:PopupControlContentControl ID="SRPopupControlContentControl" runat="server">
                                                                        <div id="divSR" style="padding: 20px; background-color: rgb(237,243,244)">
                                                                            <div style="margin-bottom: 10px; width: 430px;">
                                                                                <div style="float: left; width: 130px;">
                                                                                    Type:<span style="color: red"> *</span>
                                                                                </div>
                                                                                <div style="float: left;">
                                                                                    <dxe:ASPxComboBox ID="cmbSR" ClientInstanceName="ccmbSR" runat="server" Width="170px" EnableIncrementalFiltering="True"
                                                                                        Height="25px" ValueType="System.String" AutoPostBack="false" EnableSynchronization="False">
                                                                                        <Items>
                                                                                            <%-- <dxe:ListEditItem Text="select" />--%>
                                                                                            <dxe:ListEditItem Text="Pan Card" Value="1" />
                                                                                            <dxe:ListEditItem Text="Passport" Value="2" />
                                                                                            <dxe:ListEditItem Text="Ration Card" Value="3" />
                                                                                            <dxe:ListEditItem Text="Driving License" Value="4" />
                                                                                            <dxe:ListEditItem Text="VoterId" Value="5" />
                                                                                            <dxe:ListEditItem Text="Social Security Number" Value="6" />
                                                                                            <dxe:ListEditItem Text="SEBI Registration" Value="7" />
                                                                                            <dxe:ListEditItem Text="MAPIN CODE" Value="8" />
                                                                                            <dxe:ListEditItem Text="RBI Ref.No" Value="9" />
                                                                                            <dxe:ListEditItem Text="MOA" Value="10" />
                                                                                            <dxe:ListEditItem Text="FMC Registration" Value="11" />
                                                                                            <dxe:ListEditItem Text="CIN" Value="12" />
                                                                                            <dxe:ListEditItem Text="Other" Value="13" />
                                                                                            <dxe:ListEditItem Text="Aadhar Card" Value="14" />
                                                                                            <dxe:ListEditItem Text="VAT Regn No" Value="15" />
                                                                                            <dxe:ListEditItem Text="CST Regn No" Value="16" />
                                                                                            <dxe:ListEditItem Text="ECC Regn No" Value="17" />
                                                                                            <dxe:ListEditItem Text="Service Tax Regn No" Value="18" />
                                                                                            <dxe:ListEditItem Text="SGST" Value="19" />
                                                                                            <dxe:ListEditItem Text="CGST" Value="20" />
                                                                                            <dxe:ListEditItem Text="IGST" Value="21" />
                                                                                        </Items>
                                                                                        <ClientSideEvents SelectedIndexChanged="function (s, e) {fn_ComboIndexChange(s.GetValue());}" />
                                                                                    </dxe:ASPxComboBox>
                                                                                </div>
                                                                                <div id="PanCheck" style="padding-left: 10px; float: left;">
                                                                                    <dxe:ASPxCheckBox ID="chkPan" ClientInstanceName="cchkPan" runat="server" Text="PAN Exempt">
                                                                                        <ClientSideEvents CheckedChanged="function (s, e) {fn_CheckChange(s,e);}" />
                                                                                    </dxe:ASPxCheckBox>
                                                                                </div>
                                                                                <br style="clear: both;" />
                                                                                <br style="clear: both;" />
                                                                                <div id="ProofType">
                                                                                    <div style="float: left; width: 130px;">
                                                                                        Proof Type: &nbsp
                                                                                    </div>
                                                                                    <div>
                                                                                        <dxe:ASPxComboBox ID="CmbProofType" ClientInstanceName="cCmbProofType" runat="server"
                                                                                            Width="170px" Height="25px" ValueType="System.String" AutoPostBack="false" EnableSynchronization="False">
                                                                                            <Items>
                                                                                                <dxe:ListEditItem Text="Mapin ID" Value="1"></dxe:ListEditItem>
                                                                                                <dxe:ListEditItem Text="Passport" Value="2"></dxe:ListEditItem>
                                                                                                <dxe:ListEditItem Text="Voter ID" Value="3"></dxe:ListEditItem>
                                                                                                <dxe:ListEditItem Text="Driving License" Value="4"></dxe:ListEditItem>
                                                                                                <dxe:ListEditItem Text="Ration Card" Value="5"></dxe:ListEditItem>
                                                                                                <dxe:ListEditItem Text="Registration Details" Value="6"></dxe:ListEditItem>
                                                                                                <dxe:ListEditItem Text="UID (Aadhar ID)" Value="7"></dxe:ListEditItem>
                                                                                            </Items>
                                                                                        </dxe:ASPxComboBox>
                                                                                    </div>
                                                                                </div>
                                                                                <br style="clear: both;" />
                                                                                <div id="ProofNumber">
                                                                                    <div style="float: left; width: 130px;">
                                                                                        Proof Number: &nbsp;
                                                                                    </div>
                                                                                    <div>
                                                                                        <dxe:ASPxTextBox ID="CmbProofNumber" ClientInstanceName="cCmbProofNumber" ClientEnabled="true"
                                                                                            runat="server" Height="25px" Width="170px" MaxLength="30">
                                                                                        </dxe:ASPxTextBox>
                                                                                    </div>
                                                                                </div>
                                                                                <br style="clear: both;" />
                                                                                <div id="Number">
                                                                                    <div style="float: left; width: 130px;">
                                                                                         Number:&nbsp
                                                                                    </div>
                                                                                    <div style="display: inline-flex;">
                                                                                        <dxe:ASPxTextBox ID="txtNumber" ClientInstanceName="ctxtNumber" ClientEnabled="true" MaxLength="50" va
                                                                                            runat="server" Height="25px" Width="170px">
                                                                                           
                                                                                        </dxe:ASPxTextBox>

                                                                                        <dxe:ASPxLabel ID="labelformat" Style="margin-left: 20px" Width="100px" runat="server" CssClass="formatcss" ClientInstanceName="lbformat" Text="Format: XXXXX9999X"></dxe:ASPxLabel>

                                                                                    </div>
                                                                                </div>
                                                                                <br style="clear: both;" />
                                                                                <div id="PlaceIssue">
                                                                                    <div style="float: left; width: 130px;">
                                                                                        Place of Issue: &nbsp;
                                                                                    </div>
                                                                                    <div>
                                                                                        <dxe:ASPxTextBox ID="txtIssuePlace" ClientInstanceName="ctxtIssuePlace" ClientEnabled="true" MaxLength="50"
                                                                                            runat="server" Height="25px" Width="170px">
                                                                                        </dxe:ASPxTextBox>
                                                                                    </div>
                                                                                </div>
                                                                                <br style="clear: both;" />
                                                                                <div id="dateofissue">
                                                                                    <div style="float: left; width: 130px;">
                                                                                        Date Of Issue:&nbsp;
                                                                                    </div>
                                                                                    <div>
                                                                                        <%--<dxe:ASPxDateEdit ID="ASPxDateEdit5"   runat="server" Height="25px">
                                    <ClientSideEvents Init="function(s,e){ s.SetDate(new Date());}" />
                                </dxe:ASPxDateEdit>--%>
                                                                                        <dxe:ASPxDateEdit ID="dtIssue" runat="server" ClientInstanceName="cdtIssue" DateOnError="Today"
                                                                                            EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" Width="170px"
                                                                                            Height="25px" Font-Size="11px" TabIndex="0">
                                                                                        </dxe:ASPxDateEdit>
                                                                                    </div>
                                                                                </div>
                                                                                <br style="clear: both;" />
                                                                                <div id="validuntill">
                                                                                    <div style="float: left; width: 130px;">
                                                                                        Valid Until: &nbsp;
                                                                                    </div>
                                                                                    <div>
                                                                                        <%--<dxe:ASPxDateEdit ID="ASPxDateEdit4" ClientInstanceName="ctxtSaveAs" ClientEnabled="true" 
                                    runat="server" Height="25px">
                                 </dxe:ASPxDateEdit>--%>
                                                                                        <dxe:ASPxDateEdit ID="dtValid" runat="server" ClientInstanceName="cdtValid" DateOnError="Today"
                                                                                            EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" Width="170px"
                                                                                            Height="25px" Font-Size="11px" TabIndex="0">
                                                                                        </dxe:ASPxDateEdit>
                                                                                    </div>
                                                                                </div>

                                                                                <br style="clear: both;" />
                                                                                <div>
                                                                                    <div style="float: left; width: 130px; height: 30px;">
                                                                                        Registration Authority Name Remarks: &nbsp;
                                                                                    </div>
                                                                                    <div>
                                                                                        <dxe:ASPxTextBox ID="txtRegAuthName" ClientInstanceName="ctxtRegAuthName" ClientEnabled="true"
                                                                                            runat="server" Height="25px" Width="170px" MaxLength="50">
                                                                                        </dxe:ASPxTextBox>
                                                                                    </div>
                                                                                </div>
                                                                                <br style="clear: both;" />
                                                                                <div>
                                                                                    <div style="float: left; width: 130px;">
                                                                                        Verify:&nbsp;
                                                                                    </div>
                                                                                    <div>
                                                                                        <dxe:ASPxComboBox ID="cmbVerify" ClientInstanceName="ccmbVerify" runat="server" Width="170px"
                                                                                            Height="25px" ValueType="System.String" AutoPostBack="false" EnableSynchronization="False"
                                                                                            SelectedIndex="0">
                                                                                            <Items>
                                                                                                <dxe:ListEditItem Text="Verified" Value="1"></dxe:ListEditItem>
                                                                                                <dxe:ListEditItem Text="Not Verified" Value="2"></dxe:ListEditItem>
                                                                                            </Items>
                                                                                            <%--<ClientSideEvents SelectedIndexChanged="function (s, e) {fn_ComboIndexChange(s,e);}" />--%>
                                                                                        </dxe:ASPxComboBox>
                                                                                    </div>
                                                                                </div>
                                                                                <br style="clear: both;" />
                                                                            </div>
                                                                            <div style="text-align: center">
                                                                                <div style="display: inline-block">
                                                                                    <dxe:ASPxButton ID="btnSave_Registration" ClientInstanceName="cbtnSave_Registration"
                                                                                        runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                                                                                        <ClientSideEvents Click="function (s, e) {btnSave_Registration();}" />
                                                                                    </dxe:ASPxButton>
                                                                                </div>
                                                                                <div style="display: inline-block">
                                                                                    <dxe:ASPxButton ID="btnCancel_Registration" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger">
                                                                                        <ClientSideEvents Click="function (s, e) {fn_btnCancel();}" />
                                                                                    </dxe:ASPxButton>
                                                                                </div>
                                                                                <br style="clear: both;" />
                                                                            </div>
                                                                            <br style="clear: both;" />
                                                                        </div>
                                                                    </dxe:PopupControlContentControl>
                                                                </ContentCollection>
                                                                <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                                                            </dxe:ASPxPopupControl>
                                                        </dxe:ContentControl>
                                                    </ContentCollection>
                                                </dxe:TabPage>
                                                <dxe:TabPage Name="Exchange/DP Segment Registrations" Visible="false" Text="Exchange/DP Segment Registrations">

                                                    <ContentCollection>
                                                        <dxe:ContentControl runat="server">

                                                            <div style="float: right;">

                                                                <a href="javascript:void(0);" class="btn btn-primary" onclick="exchange.AddNewRow();"><span>Add New</span> </a>
                                                            </div>

                                                            <dxe:ASPxGridView ID="gridExchange" ClientInstanceName="exchange" Width="100%"
                                                                runat="server" AutoGenerateColumns="False" KeyFieldName="crg_internalId" DataSourceID="SqlExchange"
                                                                OnRowValidating="gridExchange_RowValidating" OnInitNewRow="gridExchange_InitNewRow"
                                                                OnRowInserting="gridExchange_RowInserting" OnStartRowEditing="gridExchange_StartRowEditing"
                                                                OnRowUpdating="gridExchange_RowUpdating" OnRowDeleting="gridExchange_RowDeleting"
                                                                OnRowDeleted="gridExchange_RowDeleted" OnCustomJSProperties="gridExchange_CustomJSProperties"
                                                                OnHtmlEditFormCreated="gridExchange_HtmlEditFormCreated">
                                                                <SettingsSearchPanel Visible="True" />
                                                                <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                                                <Columns>
                                                                    <dxe:GridViewDataTextColumn FieldName="crg_internalId" Visible="false">
                                                                        <EditFormSettings Visible="false" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="crg_cntID" Visible="false">
                                                                        <EditFormSettings Visible="false" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataComboBoxColumn Caption="Company" FieldName="crg_company1" VisibleIndex="0">
                                                                        <PropertiesComboBox DataSourceID="SqlComp" ValueField="cmp_internalid" TextField="cmp_Name"
                                                                            ValueType="System.String" EnableIncrementalFiltering="true" EnableSynchronization="False">
                                                                        </PropertiesComboBox>
                                                                        <EditFormSettings Visible="false" VisibleIndex="0" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>
                                                                    <dxe:GridViewDataComboBoxColumn FieldName="crg_exchange1" Caption="Exchange Segment"
                                                                        Visible="true" VisibleIndex="1">
                                                                        <PropertiesComboBox DataSourceID="SqlExchangeSegment" ValueField="exch_internalId"
                                                                            TextField="Exchange" ValueType="System.String" EnableIncrementalFiltering="true"
                                                                            EnableSynchronization="False">
                                                                        </PropertiesComboBox>
                                                                        <EditFormSettings Visible="false" VisibleIndex="1" Caption="Exchange Segment" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="crg_company" Visible="false">
                                                                        <EditFormSettings Visible="false" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="crg_tcode" VisibleIndex="2" Caption="UCC">
                                                                        <EditFormSettings Caption="Client Id(UCC)" Visible="false" VisibleIndex="2" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="crg_regisDate1" ReadOnly="True" VisibleIndex="3"
                                                                        Caption="Regn. Date">
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataDateColumn FieldName="crg_regisDate" Visible="False" VisibleIndex="3">
                                                                        <PropertiesDateEdit EditFormat="Custom" EditFormatString="dd MM yyyy" UseMaskBehavior="True">
                                                                        </PropertiesDateEdit>
                                                                        <EditFormSettings Caption="Registration Date" Visible="false" VisibleIndex="3" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataDateColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="crg_businessCmmDate1" ReadOnly="True" VisibleIndex="4"
                                                                        Caption="Buss. Comm Date">
                                                                        <EditFormSettings Visible="False" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataDateColumn FieldName="crg_businessCmmDate" Visible="False" VisibleIndex="4">
                                                                        <PropertiesDateEdit EditFormat="Custom" EditFormatString="dd MM yyyy" UseMaskBehavior="True">
                                                                        </PropertiesDateEdit>
                                                                        <EditFormSettings Caption="Business Comm. Date" Visible="True" VisibleIndex="4" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataDateColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="crg_suspensionDate1" ReadOnly="True" VisibleIndex="5"
                                                                        Caption="Sus. Date">
                                                                        <EditFormSettings Visible="False" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataDateColumn FieldName="crg_suspensionDate" Visible="False" VisibleIndex="5">
                                                                        <PropertiesDateEdit EditFormat="Custom" EditFormatString="dd MM yyyy" UseMaskBehavior="True">
                                                                        </PropertiesDateEdit>
                                                                        <EditFormSettings Caption="Suspension Date" Visible="false" VisibleIndex="5" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataDateColumn>
                                                                    <dxe:GridViewDataMemoColumn FieldName="crg_reasonforSuspension" Visible="False"
                                                                        VisibleIndex="5">
                                                                        <EditFormSettings Caption="Reason For Suspension" Visible="false" VisibleIndex="6" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataMemoColumn>
                                                                    <dxe:GridViewCommandColumn VisibleIndex="6" ShowEditButton="true" ShowDeleteButton="true" HeaderStyle-HorizontalAlign="Center">

                                                                        <HeaderTemplate>
                                                                            Actions
                                                                        </HeaderTemplate>
                                                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                    </dxe:GridViewCommandColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="crg_SubBrokerFranchiseeID" Visible="false">
                                                                        <EditFormSettings Visible="false" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="crg_Dealer" Visible="false">
                                                                        <EditFormSettings Visible="false" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataDateColumn FieldName="crg_AccountClosureDate" Visible="False">
                                                                        <EditFormSettings Visible="false" />
                                                                    </dxe:GridViewDataDateColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="crg_FrontOfficeBranchCode" Visible="false">
                                                                        <EditFormSettings Visible="false" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="crg_FrontOfficeGroupCode" Visible="false">
                                                                        <EditFormSettings Visible="false" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="crg_ParticipantSchemeCode" Visible="false">
                                                                        <EditFormSettings Visible="false" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="crg_ClearingBankCode" Visible="false">
                                                                        <EditFormSettings Visible="false" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="crg_SchemeCode" Visible="false">
                                                                        <EditFormSettings Visible="false" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="crg_STTPattern" Visible="false">
                                                                        <EditFormSettings Visible="false" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="crg_swapucc" Visible="false">
                                                                        <EditFormSettings Visible="false" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                </Columns>
                                                                <SettingsCommandButton>
                                                                    <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Image-ToolTip="Modify">
                                                                        <Image AlternateText="Edit" Url="../../../assests/images/Edit.png"></Image>
                                                                    </EditButton>
                                                                    <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete" Image-ToolTip="Delete">
                                                                        <Image AlternateText="Delete" Url="../../../assests/images/Delete.png"></Image>
                                                                    </DeleteButton>

                                                                    <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary">
                                                                        <Styles>
                                                                            <Style CssClass="btn btn-primary"></Style>
                                                                        </Styles>
                                                                    </UpdateButton>
                                                                    <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger">
                                                                        <Styles>
                                                                            <Style CssClass="btn btn-danger"></Style>
                                                                        </Styles>
                                                                    </CancelButton>
                                                                </SettingsCommandButton>
                                                                <ClientSideEvents EndCallback="function(s,e){DrpValueSelect(s.cpValue);}" />
                                                                <Styles>
                                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                    </Header>
                                                                    <LoadingPanel ImageSpacing="10px">
                                                                    </LoadingPanel>
                                                                </Styles>
                                                                <StylesEditors>
                                                                    <ProgressBar Height="25px">
                                                                    </ProgressBar>
                                                                </StylesEditors>
                                                                <SettingsEditing PopupEditFormHeight="350px" PopupEditFormHorizontalAlign="Center"
                                                                    PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="450px" EditFormColumnCount="1" />
                                                                <Styles>
                                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                    </Header>
                                                                    <LoadingPanel ImageSpacing="10px">
                                                                    </LoadingPanel>
                                                                </Styles>
                                                                <SettingsSearchPanel Visible="True" />
                                                                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                                                <SettingsText PopupEditFormCaption="Add/Modify Exchange" ConfirmDelete="Confirm delete?" />
                                                                <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                                                                    <FirstPageButton Visible="True">
                                                                    </FirstPageButton>
                                                                    <LastPageButton Visible="True">
                                                                    </LastPageButton>
                                                                </SettingsPager>
                                                                <SettingsBehavior ColumnResizeMode="NextColumn" AllowFocusedRow="true" ConfirmDelete="True" />
                                                                <Templates>
                                                                    <TitlePanel>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td align="center" style="width: 50%">
                                                                                    <span style="color: White; font-weight: bold; font-size: 12px; text-align: center">Exchange/DP
                                                                                    Segment Registrations</span>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </TitlePanel>
                                                                    <EditForm>
                                                                        <div style="background-color: rgb(237,243,244); color: #000; margin: 8px">
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td class="EcoheadCon_" style="text-align: left;">Company :
                                                                                    </td>
                                                                                    <td style="text-align: left">
                                                                                        <dxe:ASPxComboBox ID="compCompany" runat="server" DataSourceID="SqlComp" ValueField="cmp_internalid"
                                                                                            TextField="cmp_Name" ClientInstanceName="compCompany" EnableIncrementalFiltering="true"
                                                                                            EnableSynchronization="False" ValueType="System.String" Value='<%# Bind("crg_company1") %>'
                                                                                            Width="233px">
                                                                                            <ButtonStyle Width="13px">
                                                                                            </ButtonStyle>
                                                                                            <ClientSideEvents SelectedIndexChanged="function(s,e){change()}" />
                                                                                        </dxe:ASPxComboBox>
                                                                                    </td>
                                                                                    <td class="EcoheadCon_" style="text-align: left;">Segment :
                                                                                    </td>
                                                                                    <td class="EcoheadCon_" style="text-align: left; font-size: medium; color: Red; width: 8px;"></td>
                                                                                    <td style="text-align: left">
                                                                                        <dxe:ASPxComboBox ID="compSegment" runat="server" DataSourceID="SqlSegment" ValueField="exch_internalId"
                                                                                            TextField="Exchange" ClientInstanceName="compSegment" EnableIncrementalFiltering="true"
                                                                                            EnableSynchronization="False" OnCallback="compSegment_Callback" ValueType="System.String"
                                                                                            Width="233px">
                                                                                            <ButtonStyle Width="13px">
                                                                                            </ButtonStyle>
                                                                                        </dxe:ASPxComboBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="EcoheadCon_" style="text-align: left;">Client ID(UCC) :
                                                                                    </td>
                                                                                    <td style="text-align: left">
                                                                                        <asp:TextBox ID="txtClientUcc" runat="server" Text='<%# Bind("crg_tcode") %>' Width="230px"></asp:TextBox>
                                                                                    </td>
                                                                                    <td class="EcoheadCon_" style="text-align: left;">Registration Date :
                                                                                    </td>
                                                                                    <td class="EcoheadCon_" style="text-align: left; font-size: medium; color: Green; width: 8px;">*
                                                                                    </td>
                                                                                    <td style="text-align: left">
                                                                                        <dxe:ASPxDateEdit ID="dtRegistration" runat="server" Value='<%# Bind("crg_regisDate") %>'
                                                                                            EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" Width="233px">
                                                                                            <ButtonStyle Width="13px">
                                                                                            </ButtonStyle>
                                                                                        </dxe:ASPxDateEdit>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="EcoheadCon_" style="text-align: left;">Business Comm. Date :
                                                                                    </td>
                                                                                    <td style="text-align: left">
                                                                                        <dxe:ASPxDateEdit ID="dtBusienssCommDate" runat="server" Value='<%# Bind("crg_businessCmmDate") %>'
                                                                                            EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" Width="233px">
                                                                                            <ButtonStyle Width="13px">
                                                                                            </ButtonStyle>
                                                                                        </dxe:ASPxDateEdit>
                                                                                    </td>
                                                                                    <td class="EcoheadCon_" style="text-align: left;">Suspension Date :
                                                                                    </td>
                                                                                    <td class="EcoheadCon_" style="text-align: left; font-size: medium; color: Red; width: 8px;"></td>
                                                                                    <td style="text-align: left">
                                                                                        <dxe:ASPxDateEdit ID="dtSuspension" runat="server" Value='<%# Bind("crg_suspensionDate") %>'
                                                                                            EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" Width="233px">
                                                                                            <ButtonStyle Width="13px">
                                                                                            </ButtonStyle>
                                                                                        </dxe:ASPxDateEdit>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="EcoheadCon_" style="text-align: left;">Sub Broker :
                                                                                    </td>
                                                                                    <td style="text-align: left">
                                                                                        <asp:TextBox ID="txtSubBroker" runat="server" onkeyup="CallAjaxForSubBroker(this,'SearchByEmployeesSubBroker',event)"
                                                                                            Text='<%# Bind("Franchisee") %>' Width="230px"></asp:TextBox>
                                                                                        <asp:HiddenField ID="txtSubBroker_hidden" runat="server" Value='<%# Bind("crg_SubBrokerFranchiseeID") %>' />
                                                                                    </td>
                                                                                    <td class="EcoheadCon_" style="text-align: left;">Dealer :
                                                                                    </td>
                                                                                    <td class="EcoheadCon_" style="text-align: left; font-size: medium; color: Red; width: 8px;"></td>
                                                                                    <td style="text-align: left">
                                                                                        <asp:TextBox ID="txtDealer" runat="server" onkeyup="CallAjaxForDealer(this,'SearchByContact',event)"
                                                                                            Text='<%# Bind("Dealer") %>' Width="230px"></asp:TextBox>
                                                                                        <asp:HiddenField ID="txtDealer_hidden" runat="server" Value='<%# Bind("crg_Dealer") %>' />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="EcoheadCon_" style="text-align: left;">Closer Date :
                                                                                    </td>
                                                                                    <td style="text-align: left">
                                                                                        <dxe:ASPxDateEdit ID="dtCloser" runat="server" Value='<%# Bind("crg_AccountClosureDate") %>'
                                                                                            EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" Width="233px">
                                                                                            <ButtonStyle Width="13px">
                                                                                            </ButtonStyle>
                                                                                        </dxe:ASPxDateEdit>
                                                                                    </td>
                                                                                    <td class="EcoheadCon_" style="text-align: left;">Front Office Branch :
                                                                                    </td>
                                                                                    <td class="EcoheadCon_" style="text-align: left; font-size: medium; color: Red; width: 8px;"></td>
                                                                                    <td style="text-align: left">
                                                                                        <asp:TextBox ID="txtFOfficeBranch" runat="server" Text='<%# Bind("crg_FrontOfficeBranchCode") %>'
                                                                                            Width="230px"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="EcoheadCon_" style="text-align: left;">Front Office Group :
                                                                                    </td>
                                                                                    <td style="text-align: left">
                                                                                        <asp:TextBox ID="txtFOfficeGroup" runat="server" Text='<%# Bind("crg_FrontOfficeGroupCode") %>'
                                                                                            Width="230px"></asp:TextBox>
                                                                                    </td>
                                                                                    <td class="EcoheadCon_" style="text-align: left;">Participant Code :
                                                                                    </td>
                                                                                    <td class="EcoheadCon_" style="text-align: left; font-size: medium; color: Green; width: 8px;">*
                                                                                    </td>
                                                                                    <td style="text-align: left">
                                                                                        <asp:TextBox ID="txtParticipantCode" runat="server" Text='<%# Bind("crg_ParticipantSchemeCode") %>'
                                                                                            Width="230px"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="EcoheadCon_" style="text-align: left;">Clear Bank code :
                                                                                    </td>
                                                                                    <td style="text-align: left">
                                                                                        <asp:TextBox ID="txtBankCode" runat="server" Text='<%# Bind("crg_ClearingBankCode") %>'
                                                                                            Width="230px"></asp:TextBox>
                                                                                    </td>
                                                                                    <td class="EcoheadCon_" style="text-align: left;">Scheme Code :
                                                                                    </td>
                                                                                    <td class="EcoheadCon_" style="text-align: left; font-size: medium; color: Red; width: 8px;"></td>
                                                                                    <td style="text-align: left">
                                                                                        <asp:TextBox ID="txtSchemeCode" runat="server" Text='<%# Bind("crg_SchemeCode") %>'
                                                                                            Width="230px"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="EcoheadCon_" style="text-align: left;">STT Patern :
                                                                                    </td>
                                                                                    <td style="text-align: left">
                                                                                        <asp:DropDownList ID="ddlPatern" runat="server" Width="233px">
                                                                                            <asp:ListItem Text="Day" Value="D"></asp:ListItem>
                                                                                            <asp:ListItem Text="Contract Note" Value="C"></asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td class="EcoheadCon_" style="text-align: left;">reason For Suspension :
                                                                                    </td>
                                                                                    <td class="EcoheadCon_" style="text-align: left; font-size: medium; color: Red; width: 8px;"></td>
                                                                                    <td style="text-align: left">
                                                                                        <asp:TextBox ID="txtSuspension" runat="server" Text='<%# Bind("crg_reasonforSuspension") %>'
                                                                                            Width="230px"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="EcoheadCon_" style="text-align: left;">STT Purchase/Sale
                                                                                    <br />
                                                                                        Value Calculation Method :
                                                                                    </td>
                                                                                    <td style="text-align: left">
                                                                                        <asp:DropDownList ID="drpSttWap" runat="server" Width="233px">
                                                                                            <asp:ListItem Text="Weighted Average price(WAP)" Value="W"></asp:ListItem>
                                                                                            <asp:ListItem Text="Actual Value" Value="A"></asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td class="EcoheadCon_" style="text-align: left;">SWAP MAPIN with SEBI (For STP Files)
                                                                                    </td>
                                                                                    <td class="EcoheadCon_" style="text-align: left; font-size: medium; color: Red; width: 8px;"></td>
                                                                                    <td style="text-align: left">
                                                                                        <dxe:ASPxComboBox ID="ComboSwapMapinSebi" runat="server" ClientInstanceName="cComboSwapMapinSebi"
                                                                                            Font-Size="12px" SelectedIndex="0" ValueType="System.String" Width="50px" EnableIncrementalFiltering="True">
                                                                                            <Items>
                                                                                                <dxe:ListEditItem Value="N" Text="No"></dxe:ListEditItem>
                                                                                                <dxe:ListEditItem Value="Y" Text="Yes"></dxe:ListEditItem>
                                                                                            </Items>
                                                                                        </dxe:ASPxComboBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr style="display: none">
                                                                                    <td colspan="4">
                                                                                        <asp:Label ID="lblSttWap" runat="Server" Text='<%#Bind("crg_STTWap") %>'></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="text-align: right;" colspan="2">
                                                                                        <dxe:ASPxButton ID="btnUpdate" runat="server" Text="Save" ToolTip="Update data"
                                                                                            Height="18px" Width="88px" AutoPostBack="False">
                                                                                            <ClientSideEvents Click="function(s, e) {exchange.UpdateEdit();}" />
                                                                                        </dxe:ASPxButton>
                                                                                    </td>
                                                                                    <td style="text-align: left;" colspan="2">
                                                                                        <dxe:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel data"
                                                                                            Height="18px" Width="88px" AutoPostBack="False">
                                                                                            <ClientSideEvents Click="function(s, e) {exchange.CancelEdit();}" />
                                                                                        </dxe:ASPxButton>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </EditForm>
                                                                </Templates>
                                                            </dxe:ASPxGridView>
                                                        </dxe:ContentControl>
                                                    </ContentCollection>
                                                </dxe:TabPage>
                                                <dxe:TabPage Name="Other Memberships" Text="Other Memberships">
                                                    <ContentCollection>
                                                        <dxe:ContentControl runat="server">
                                                            <div style="float: left;">
                                                                <% if (rights.CanAdd)
                                                                   { %>
                                                                <a href="javascript:void(0);" class="btn btn-primary" onclick="membership.AddNewRow();"><span>Add New</span> </a>
                                                                <% } %>
                                                            </div>
                                                            <dxe:ASPxGridView ID="gridMembership" ClientInstanceName="membership" runat="server"
                                                                Width="100%" AutoGenerateColumns="False" DataSourceID="Sqlmembership" KeyFieldName="crg_internalid" OnRowUpdated="gridMembership_RowUpdated"
                                                                OnRowDeleted="gridMembership_RowDeleted" OnRowValidating="gridMembership_RowValidating"  OnCommandButtonInitialize="gridMembership_CommandButtonInitialize">

                                                                <Styles>
                                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                    </Header>
                                                                    <LoadingPanel ImageSpacing="10px">
                                                                    </LoadingPanel>
                                                                </Styles>
                                                                <StylesEditors>
                                                                    <ProgressBar Height="25px">
                                                                    </ProgressBar>
                                                                </StylesEditors>
                                                                <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="355px" PopupEditFormHorizontalAlign="WindowCenter"
                                                                    PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="580px"
                                                                    EditFormColumnCount="1" />
                                                                <%--<Styles>
                                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                    </Header>
                                                                    <LoadingPanel ImageSpacing="10px">
                                                                    </LoadingPanel>
                                                                </Styles>--%>
                                                                <%--<Settings ShowStatusBar="Visible" ShowTitlePanel="True" />--%>
                                                                <SettingsText PopupEditFormCaption="Add/Modify Membership" ConfirmDelete="Confirm delete?" />
                                                                <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                                                                    <FirstPageButton Visible="True">
                                                                    </FirstPageButton>
                                                                    <LastPageButton Visible="True">
                                                                    </LastPageButton>
                                                                </SettingsPager>
                                                                <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                                                                <Templates>
                                                                    <TitlePanel>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td align="center" style="width: 100%">
                                                                                    <span class="Ecoheadtxt" style="color: Black">Other
                                                                                    Memberships</span>
                                                                                </td>
                                                                                <%--<td align="right">
                                                                            <table width="200">
                                                                                <tr>
                                                                                    
                                                                                    <td>
                                                                                      
                                                                                    </td>
                                                                                    <td>
                                                                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" Text="ADD" ToolTip="Add New Data"   Height="18px" Width="88px" AutoPostBack="False">
                                                                                            <clientsideevents click="function(s, e) {membership.AddNewRow();}" />
                                                                                        </dxe:ASPxButton>
                                                                                    </td>
                                                                                                                        
                                                                                     
                                                                                  </tr>
                                                                              </table>
                                                                          </td>   --%>
                                                                            </tr>
                                                                        </table>
                                                                    </TitlePanel>
                                                                    <EditForm>
                                                                        <div style="background-color: rgb(237,243,244); color: #000; margin: 8px 8px 0px 8px">
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td style="width: 5%"></td>
                                                                                    <td style="width: 90%">

                                                                                        <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors"></dxe:ASPxGridViewTemplateReplacement>

                                                                                        <div style="padding: 2px 2px 2px 155px">
                                                                                            <div class="dxbButton" style="display: inline-block; padding: 3px">
                                                                                                <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                                                                    runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                            </div>
                                                                                            <div class="dxbButton" style="display: inline-block; padding: 3px">
                                                                                                <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                                                                    runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                            </div>
                                                                                        </div>
                                                                                    </td>
                                                                                    <td style="width: 5%"></td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </EditForm>
                                                                </Templates>
                                                                <SettingsSearchPanel Visible="True" />
                                                                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />




                                                                <Columns>
                                                                    <dxe:GridViewDataTextColumn FieldName="crg_internalid" ReadOnly="True" Visible="False"
                                                                        VisibleIndex="0">
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>


                                                                    <dxe:GridViewDataTextColumn FieldName="crg_cntId" Visible="False" VisibleIndex="0">
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>

                                                                    <dxe:GridViewDataComboBoxColumn Caption="Professional Association" FieldName="crg_idprof" VisibleIndex="0">

                                                                        <PropertiesComboBox DataSourceID="SqlProfessional" ValueField="prof_id" TextField="prof_name"
                                                                            ValueType="System.String" EnableSynchronization="False" EnableIncrementalFiltering="true" Width="90%">
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                <RequiredField IsRequired="True" ErrorText="Mandatory"/>
                                                                            </ValidationSettings>
                                                                        </PropertiesComboBox>

                                                                        <EditFormSettings Caption="Professional Association" Visible="True" VisibleIndex="0" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>


                                                                    <dxe:GridViewDataTextColumn Caption="Membership Number" FieldName="crg_memNumber"
                                                                        VisibleIndex="1">
                                                                        <PropertiesTextEdit MaxLength="200" Width="90%">
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                <RequiredField IsRequired="True" ErrorText="Mandatory"/>
                                                                            </ValidationSettings>
                                                                        </PropertiesTextEdit>
                                                                        <EditFormSettings Visible="True" VisibleIndex="1" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>


                                                                    <dxe:GridViewDataTextColumn Caption="Membership Type" FieldName="crg_memtype" VisibleIndex="2">
                                                                        <PropertiesTextEdit MaxLength="200" Width="82%"></PropertiesTextEdit>
                                                                        <EditFormSettings Visible="True" VisibleIndex="2" />

                                                                        <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>


                                                                    <%-- ---------------------------------------------%>
                                                                    <dxe:GridViewDataTextColumn Caption="Validity Type" FieldName="crg_validityType" VisibleIndex="2">
                                                                        <EditFormSettings Visible="false" VisibleIndex="2" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>



                                                                    <dxe:GridViewDataComboBoxColumn Caption="Validity Type" FieldName="crg_validityType"
                                                                        VisibleIndex="2" Visible="false">
                                                                        <PropertiesComboBox ValueType="System.String" EnableSynchronization="False" EnableIncrementalFiltering="true" Width="82%">
                                                                            <ClientSideEvents Init="function(s,e) {
                                                                            var value1 = s.GetValue();  
                                                                            if(value1 == &quot;Limited&quot;)
                                                                            {
                                                                                membership.GetEditor(&quot;crg_memExpDate&quot;).SetEnabled(true); 
                                                                            }
                                                                            else
                                                                            {
                                                                                membership.GetEditor(&quot;crg_memExpDate&quot;).SetEnabled(false); 
                                                                            }
                                                                                }" />
                                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) {
	                                                                        var value = s.GetValue();
                                                                            if(value == &quot;Limited&quot;)
                                                                            {
                                                                                membership.GetEditor(&quot;crg_memExpDate&quot;).SetEnabled(true);
                                                                                
                                                                            }
                                                                            else
                                                                            {
                                                                                membership.GetEditor(&quot;crg_memExpDate&quot;).SetEnabled(false);
                                                                                
                                                                            }
                                                                        }" />
                                                                            <Items>
                                                                                <dxe:ListEditItem Text="Limited" Value="Limited"></dxe:ListEditItem>
                                                                                <dxe:ListEditItem Text="Lifetime" Value="Lifetime"></dxe:ListEditItem>
                                                                            </Items>
                                                                        </PropertiesComboBox>

                                                                        <EditFormSettings Caption="Validity Type" Visible="True" VisibleIndex="3" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>


                                                                    <dxe:GridViewDataDateColumn Caption="Membership Expiry Date" FieldName="crg_memExpDate"
                                                                        Visible="true">

                                                                        <%--                                                                            <HeaderTemplate>
                                                                              <dxe:ASPxLabel ID="lblCategoryId" runat="server" ClientInstanceName="lblx" Text="Membership Expiry Date" />
                                                                                </HeaderTemplate>--%>
                                                                        <%--EditFormat="Custom" EditFormatString="dd MM yyyy" UseMaskBehavior="True"--%>
                                                                        <PropertiesDateEdit DateOnError="Today" EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="false" Width="82%" DisplayFormatString="dd MMM yyyy">
                                                                        </PropertiesDateEdit>
                                                                        <EditFormSettings Caption="Membership Expiry Date" Visible="True" VisibleIndex="4" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <HeaderStyle CssClass="unitPriceColumn" />
                                                                        <EditCellStyle CssClass="unitPriceColumn" />
                                                                        <FilterCellStyle CssClass="unitPriceColumn" />
                                                                        <FooterCellStyle CssClass="unitPriceColumn" />
                                                                        <GroupFooterCellStyle CssClass="unitPriceColumn" />
                                                                        <CellStyle CssClass="gridcellleft unitPriceColumn">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataDateColumn>
                                                                   <%-- <dxe:GridViewDataTextColumn FieldName="crg_memExpDate1" Caption="Membership Expiry Date"
                                                                        VisibleIndex="3">
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>--%>
                                                                    <dxe:GridViewDataTextColumn Caption="Notes" FieldName="crg_notes" VisibleIndex="5" PropertiesTextEdit-MaxLength="200">
                                                                        <PropertiesTextEdit Width="82%"></PropertiesTextEdit>
                                                                        <EditFormSettings Caption="Notes" Visible="True" VisibleIndex="5" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="CreateUser" Visible="False" VisibleIndex="5">
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataDateColumn FieldName="CreateDate" Visible="False" VisibleIndex="5">
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataDateColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="LastModifyUser" Visible="False" VisibleIndex="5">
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataDateColumn FieldName="LastModifyDate" Visible="False" VisibleIndex="5">
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataDateColumn>
                                                                    <dxe:GridViewCommandColumn VisibleIndex="6" ShowEditButton="true" ShowDeleteButton="true" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" Width="6%">
                                                                        <HeaderTemplate>
                                                                            Actions 
                                                                        </HeaderTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                    </dxe:GridViewCommandColumn>
                                                                </Columns>
                                                                <SettingsCommandButton>
                                                                    <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                                                                        <Image AlternateText="Edit" Url="../../../assests/images/Edit.png"></Image>

                                                                        <Styles>
                                                                            <Style CssClass="pad"></Style>
                                                                        </Styles>
                                                                    </EditButton>
                                                                    <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                                        <Image AlternateText="Delete" Url="../../../assests/images/Delete.png"></Image>
                                                                    </DeleteButton>
                                                                    <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary">
                                                                        <Styles>
                                                                            <Style CssClass="btn btn-primary"></Style>
                                                                        </Styles>
                                                                    </UpdateButton>
                                                                    <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger">
                                                                        <Styles>
                                                                            <Style CssClass="btn btn-danger"></Style>
                                                                        </Styles>
                                                                    </CancelButton>
                                                                </SettingsCommandButton>
                                                            </dxe:ASPxGridView>
                                                            <asp:SqlDataSource ID="Sqlmembership" runat="server" 
                                                                DeleteCommand="DELETE FROM [tbl_master_contactMembership] WHERE [crg_internalid] = @crg_internalid "
                                                                InsertCommand="INSERT INTO [tbl_master_contactMembership] ([crg_cntId],[crg_idprof], [crg_memNumber], [crg_memtype], [crg_validityType], [crg_memExpDate], [crg_notes], [CreateUser], [CreateDate]) VALUES (@crg_cntId,@crg_idprof, @crg_memNumber, @crg_memtype, @crg_validityType, @crg_memExpDate, @crg_notes, @CreateUser, getdate())"
                                                                SelectCommand="SELECT *,convert(varchar(11),crg_memExpDate,113) as crg_memExpDate1 FROM [tbl_master_contactMembership] where crg_cntId=@crg_cntId"
                                                                UpdateCommand="UPDATE [tbl_master_contactMembership] SET  [crg_idprof] = @crg_idprof, [crg_memNumber] = @crg_memNumber, [crg_memtype] = @crg_memtype, [crg_validityType] = @crg_validityType, [crg_memExpDate] = @crg_memExpDate, [crg_notes] = @crg_notes,  [LastModifyUser] = @LastModifyUser, [LastModifyDate] = getdate() WHERE [crg_internalid] = @crg_internalid">
                                                                <DeleteParameters>
                                                                    <asp:Parameter Name="crg_internalid" Type="Int32" />
                                                                </DeleteParameters>
                                                                <SelectParameters>
                                                                    <asp:SessionParameter Name="crg_cntId" SessionField="KeyVal_InternalID" Type="string" />
                                                                </SelectParameters>
                                                                <InsertParameters>
                                                                    <asp:SessionParameter Name="crg_cntId" SessionField="KeyVal_InternalID" Type="string" />
                                                                    <asp:Parameter Name="crg_idprof" Type="String" />
                                                                    <asp:Parameter Name="crg_memNumber" Type="String" />
                                                                    <asp:Parameter Name="crg_memtype" Type="String" />
                                                                    <asp:Parameter Name="crg_validityType" Type="String" />
                                                                    <asp:Parameter Type="datetime" Name="crg_memExpDate" />
                                                                    <asp:Parameter Name="crg_notes" Type="String" />
                                                                    <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="int32" />
                                                                </InsertParameters>
                                                                <UpdateParameters>
                                                                    <asp:Parameter Name="crg_idprof" Type="String" />
                                                                    <asp:Parameter Name="crg_memNumber" Type="String" />
                                                                    <asp:Parameter Name="crg_memtype" Type="String" />
                                                                    <asp:Parameter Name="crg_validityType" Type="String" />
                                                                    <asp:Parameter Type="datetime" Name="crg_memExpDate" />
                                                                    <asp:Parameter Name="crg_notes" Type="String" />
                                                                    <asp:SessionParameter Name="LastModifyUser" SessionField="userid" Type="int32" />
                                                                    <asp:Parameter Name="crg_internalid" Type="Int32" />
                                                                </UpdateParameters>
                                                            </asp:SqlDataSource>
                                                        </dxe:ContentControl>
                                                    </ContentCollection>
                                                </dxe:TabPage>
                                            </TabPages>
                                        </dxe:ASPxPageControl>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Group" Text="Group Member">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>

                            <dxe:TabPage Name="TDS" Text="TDS">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                        </TabPages>
                        <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
	                                            var Tab0 = page.GetTab(0);
	                                            var Tab1 = page.GetTab(1);
	                                            var Tab2 = page.GetTab(2);
	                                            var Tab3 = page.GetTab(3);
	                                            var Tab4 = page.GetTab(4);
	                                            var Tab5 = page.GetTab(5);
	                                            var Tab6 = page.GetTab(6);
	                                           var Tab7 = page.GetTab(7);
	                                             

	                                            if(activeTab == Tab0)
	                                            {
	                                                disp_prompt('tab0');
	                                            }
	                                            if(activeTab == Tab1)
	                                            {
	                                                disp_prompt('tab1');
	                                            }
	                                            else if(activeTab == Tab2)
	                                            {
	                                                disp_prompt('tab2');
	                                            }
	                                            else if(activeTab == Tab3)
	                                            {
	                                                disp_prompt('tab3');
	                                            }
	                                            else if(activeTab == Tab4)
	                                            {
	                                                disp_prompt('tab4');
	                                            }
	                                            else if(activeTab == Tab5)
	                                            {
	                                                disp_prompt('tab5');
	                                            }
	                                            else if(activeTab == Tab6)
	                                            {
	                                                disp_prompt('tab6');
	                                            }
                                                else if(activeTab == Tab7)
	                                            {
	                                                disp_prompt('tab7');
	                                            }
	                                            
	                                            }"></ClientSideEvents>
                        <ContentStyle>
                            <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                        </ContentStyle>
                        <LoadingPanelStyle ImageSpacing="6px">
                        </LoadingPanelStyle>
                    </dxe:ASPxPageControl>
                </td>
            </tr>
            <tr>
                <td style="display: none;">
                    <asp:HiddenField runat="server" ID="hiddenedit" />
                </td>
            </tr>
        </table>
        <%-- <asp:SqlDataSource ID="Sqlstatutory" runat="server" 
         DeleteCommand="DELETE FROM [tbl_master_contactRegistration] WHERE [crg_id] = @crg_id"
            InsertCommand="INSERT INTO [tbl_master_contactRegistration] ([crg_cntId], [crg_type], [crg_Number], [crg_registrationAuthority], [crg_place], [crg_Date], [crg_validDate], [CreateDate], [CreateUser],[crg_contactType],[crg_verify],crg_PanExmptProofType,crg_PanExmptProofNumber) VALUES (@crg_cntId, @crg_type, @crg_Number, @crg_registrationAuthority, @crg_place, @crg_Date, @crg_validDate, getdate(), @CreateUser,'contact',@crg_verify,@crg_PanExmptProofType,@crg_PanExmptProofNumber)"
            SelectCommand="SELECT [crg_id], crg_PanExmptProofType,crg_PanExmptProofNumber,[crg_cntId], [crg_type], [crg_Number], [crg_registrationAuthority], [crg_place], [crg_Date],convert(varchar(11),crg_Date,113) as crg_Date1, [crg_validDate],convert(varchar(11),crg_validDate,113) as crg_validDate1,[crg_verify], [CreateDate], [CreateUser], [LastModifyDate], [LastModifyUser] FROM [tbl_master_contactRegistration] where crg_cntId=@crg_cntId"
            UpdateCommand="UPDATE [tbl_master_contactRegistration] SET  [crg_type] = @crg_type, [crg_Number] = @crg_Number,[crg_PanExmptProofType] = @crg_PanExmptProofType, [crg_PanExmptProofNumber] = @crg_PanExmptProofNumber, [crg_registrationAuthority] = @crg_registrationAuthority, [crg_place] = @crg_place,[crg_contactType]='contact', [crg_Date] = @crg_Date, [crg_validDate] = @crg_validDate, [LastModifyDate] = getdate(), [LastModifyUser] = @LastModifyUser,[crg_verify]=@crg_verify  WHERE [crg_id] = @crg_id">
            <DeleteParameters>
                <asp:Parameter Name="crg_id" Type="Decimal" />
            </DeleteParameters>
            <SelectParameters>
                <asp:SessionParameter Name="crg_cntId" SessionField="KeyVal_InternalID" Type="string" />
            </SelectParameters>
            <InsertParameters>
                <asp:SessionParameter Name="crg_cntId" SessionField="KeyVal_InternalID" Type="string" />
                <asp:Parameter Name="crg_type" Type="String" />
                <asp:Parameter Name="crg_Number" Type="String" />
                <asp:Parameter Name="crg_registrationAuthority" Type="String" />
                <asp:Parameter Name="crg_place" Type="String" />
                <asp:Parameter Type="datetime" Name="crg_Date" />
                <asp:Parameter Type="datetime" Name="crg_validDate" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
                <asp:Parameter Name="crg_verify" Type="String" />
                <asp:Parameter Name="crg_PanExmptProofType" Type="int32" />
                <asp:Parameter Name="crg_PanExmptProofNumber" Type="String" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="crg_type" Type="String" />
                <asp:Parameter Name="crg_Number" Type="String" />
                <asp:Parameter Name="crg_registrationAuthority" Type="String" />
                <asp:Parameter Name="crg_place" Type="String" />
                <asp:Parameter Type="datetime" Name="crg_Date" />
                <asp:Parameter Type="datetime" Name="crg_validDate" />
                <asp:SessionParameter Name="LastModifyUser" SessionField="userid" Type="Decimal" />
                <asp:Parameter Name="crg_id" Type="Decimal" />
                <asp:Parameter Name="crg_verify" Type="String" />
                <asp:Parameter Name="crg_PanExmptProofType" Type="int32" />
                <asp:Parameter Name="crg_PanExmptProofNumber" Type="String" />
            </UpdateParameters>
        </asp:SqlDataSource>--%>
        <asp:SqlDataSource ID="SqlExchange" runat="server"
            SelectCommand="select ce.crg_internalId as crg_internalId,ce.crg_cntID as crg_cntID,ce.crg_exchange as crg_exchange1,ce.crg_company as crg_company1,
            case crg_company when '0' then 'N/A' else (select cmp_name from tbl_master_company where cmp_internalId=ce.crg_company) end as crg_company,
            ce.crg_exchange as crg_exchange,ltrim(rtrim(ce.crg_tcode)) as crg_tcode,
            case when ce.crg_regisDate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),ce.crg_regisDate,113) end as crg_regisDate1,
            case when ce.crg_regisDate='1/1/1900 12:00:00 AM' then null else cast(ce.crg_regisDate as datetime) end as crg_regisDate,
            case when ce.crg_businessCmmDate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),ce.crg_businessCmmDate,113) end as crg_businessCmmDate1,
            case when ce.crg_businessCmmDate='1/1/1900 12:00:00 AM' then null else cast(ce.crg_businessCmmDate as datetime) end as crg_businessCmmDate,
            case when ce.crg_suspensionDate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),ce.crg_suspensionDate,113) end as crg_suspensionDate1,
            case when ce.crg_suspensionDate='1/1/1900 12:00:00 AM' then null else cast(ce.crg_suspensionDate as datetime) end as crg_suspensionDate,
            ltrim(rtrim(ce.crg_reasonforSuspension)) as crg_reasonforSuspension,ce.CreateUser as CreateUser,
            ltrim(rtrim(ce.crg_SubBrokerFranchiseeID)) as crg_SubBrokerFranchiseeID,ltrim(rtrim(ce.crg_Dealer)) as crg_Dealer,
            case when ce.crg_AccountClosureDate='1/1/1900 12:00:00 AM' then null else cast(ce.crg_AccountClosureDate as datetime) end as crg_AccountClosureDate,
            ltrim(rtrim(ce.crg_FrontOfficeBranchCode)) as crg_FrontOfficeBranchCode,ltrim(rtrim(ce.crg_FrontOfficeGroupCode)) as crg_FrontOfficeGroupCode,
            ltrim(rtrim(ce.crg_ParticipantSchemeCode)) as crg_ParticipantSchemeCode,ltrim(rtrim(ce.crg_ClearingBankCode)) as crg_ClearingBankCode,
            ltrim(rtrim(ce.crg_SchemeCode)) as crg_SchemeCode,ltrim(rtrim(ce.crg_STTPattern)) as crg_STTPattern,ce.crg_STTWap as crg_STTWap,
            (select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+
            isnull(ltrim(rtrim(cnt_lastname)),'')+' ['+isnull(ltrim(rtrim(cnt_shortName)),'')+']' from tbl_master_contact
             where cnt_internalId=ce.crg_SubBrokerFranchiseeID) as Franchisee,(select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+
             isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'')+' ['+isnull(ltrim(rtrim(cnt_ucc)),'')+']' 
             from tbl_master_contact where cnt_internalId=ce.crg_Dealer) as Dealer,Case When crg_swapUCC='Y' Then 1 Else 0 End crg_swapUCC 
             from tbl_master_contactExchange ce where crg_cntID=@crg_cntID"
            DeleteCommand="delete from tbl_master_contactExchange where crg_internalId=@crg_internalId">
            <SelectParameters>
                <asp:SessionParameter Name="crg_cntID" SessionField="KeyVal_InternalID" Type="string" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="crg_internalId" Type="string" />
            </DeleteParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlComp" runat="server"
            SelectCommand="SELECT [cmp_internalid], [cmp_Name] FROM [tbl_master_company] where cmp_internalid in(select distinct exch_compId from tbl_master_companyExchange)"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlExchangeSegment" runat="server" 
            SelectCommand="select exch_internalId,isnull(((select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+ exch_segmentId),exch_membershipType) as Exchange from tbl_master_companyExchange"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlProfessional" runat="server" 
            SelectCommand="SELECT [prof_id], [prof_name] FROM [tbl_master_profTechBodies]"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlSegment" runat="server" 
            ConflictDetection="CompareAllValues" SelectCommand=""></asp:SqlDataSource>
        <asp:HiddenField ID="hdnDuplicatePAN" runat="server" Value="N" />
        <asp:HiddenField ID="hdnBannedPAN" runat="server" Value="N" />
    </div>
</asp:Content>