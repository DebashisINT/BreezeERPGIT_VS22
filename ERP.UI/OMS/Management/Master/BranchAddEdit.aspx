<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                17-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="Branches" Language="C#" AutoEventWireup="true" Inherits="ERP.OMS.Managemnent.Master.management_Master_BranchAddEdit" CodeBehind="BranchAddEdit.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script language="javascript" type="text/javascript">
        //Gstin Changes
        function fn_AllowonlyNumeric(s, e) {
            var theEvent = e.htmlEvent || window.event;
            var key = theEvent.keyCode || theEvent.which;
            var keychar = String.fromCharCode(key);
            if (key == 9 || key == 37 || key == 38 || key == 39 || key == 40 || key == 8) { //tab/ Left / Up / Right / Down Arrow, Backspace, Delete keys
                return;
            }
            var regex = /[0-9]/;

            if (!regex.test(keychar)) {
                theEvent.returnValue = false;
                if (theEvent.preventDefault)
                    theEvent.preventDefault();
            }
        }

        function Gstin2TextChanged(s, e) {

            if (!e.htmlEvent.ctrlKey) {
                if (e.htmlEvent.key != 'Control') {
                    s.SetText(s.GetText().toUpperCase());
                }
            }

        }
        //Code for UDF Control 
        function OpenUdf() {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                var keyVal = document.getElementById('Keyval_internalId').value;
                var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=Br&&KeyVal_InternalID=' + keyVal;
                popup.SetContentUrl(url);
                popup.Show();
            }
            return true;
        }

        function udfError() {
            jAlert('UDF is set as Mandatory. Please enter values.', 'Alert', function () { OpenUdf(); });
        }
        // End Udf Code

        //rev srijeeta mantis issue 0024438
        //validation control 
        function IsDeductorDataValid() {

            var returnData = true;

            if ($('#txtAssyear').val() == "") {
                var assyr = document.getElementById('<%= txtAssyear.ClientID %>');
                 assyr.focus();
                 $('#spAssyr').css({ 'display': 'block' });
                 returnData = false;

             }
             if ($('#txtfinyr').val() == "") {
                 var assyr = document.getElementById('<%= txtfinyr.ClientID %>');
                 assyr.focus();
                 $('#spFinyr').css({ 'display': 'block' });
                 returnData = false;
             }
             if ($('#txtNamedeductor').val() == "") {
                 var assyr = document.getElementById('<%= txtNamedeductor.ClientID %>');
                 assyr.focus();
                 $('#spNamedeductor').css({ 'display': 'block' });
                 returnData = false;
             }
             if ($('#txtBranchdeduct').val() == "") {
                 var assyr = document.getElementById('<%= txtBranchdeduct.ClientID %>');
                 assyr.focus();
                 $('#spBranchdeduct').css({ 'display': 'block' });
                 returnData = false;
             }
             if ($('#txtDeductaddr1').val() == "") {
                 var assyr = document.getElementById('<%= txtDeductaddr1.ClientID %>');
                 assyr.focus();
                 $('#spDeductaddr1').css({ 'display': 'block' });
                 returnData = false;
             }
             if (ctxtDeductpin.GetText() == "") {
                 var assyr = document.getElementById('<%= txtDeductpin.ClientID %>');
                 assyr.focus();
                 $('#spDeductpin').css({ 'display': 'block' });
                 returnData = false;
             }
             if (CtxtDeductState.GetText() == "") {
                 var assyr = document.getElementById('<%= txtDeductState.ClientID %>');
                 assyr.focus();
                 $('#spDeductState').css({ 'display': 'block' });
                 returnData = false;
             }
             if ($('#txtDeductEmail').val() == "") {
                 var assyr = document.getElementById('<%= txtDeductEmail.ClientID %>');
                 assyr.focus();
                 $('#spDedEmail').css({ 'display': 'block' });
                 returnData = false;
             }
             //if (cChkdeductAddrReturn.GetChecked()==false) {

             //    $('#spdeductAddrReturn').css({ 'display': 'block' });
             //      returnData = false;
             //  }
             if ($('#txtResponsibleDeduct').val() == "") {
                 var assyr = document.getElementById('<%= txtResponsibleDeduct.ClientID %>');
                 assyr.focus();
                 $('#spResponsibleDeduct').css({ 'display': 'block' });
                 returnData = false;
             }

             if ($('#txtdeductdesig').val() == "") {
                 var assyr = document.getElementById('<%= txtdeductdesig.ClientID %>');
                 assyr.focus();
                 $('#spdeductdesig').css({ 'display': 'block' });
                 returnData = false;
             }

             if ($('#txtPersaddr1').val() == "") {
                 var assyr = document.getElementById('<%= txtdeductdesig.ClientID %>');
                 assyr.focus();
                 $('#spPersaddr1').css({ 'display': 'block' });
                 returnData = false;
             }
             if (CtxtpersPin.GetText() == "") {
                 var assyr = document.getElementById('<%= txtpersPin.ClientID %>');
                 assyr.focus();
                 $('#spPersPin').css({ 'display': 'block' });
                 returnData = false;
             }
             if (CtxtPersState.GetText() == "") {
                 var assyr = document.getElementById('<%= txtPersState.ClientID %>');
                 assyr.focus();
                 $('#spPersState').css({ 'display': 'block' });
                 returnData = false;
             }
             if ($('#txtPersemail').val() == "") {
                 var assyr = document.getElementById('<%= txtPersemail.ClientID %>');
                 assyr.focus();
                 $('#spPersEmail').css({ 'display': 'block' });
                 returnData = false;
             }
             if ($('#txtMobile').val() == "") {
                 var assyr = document.getElementById('<%= txtMobile.ClientID %>');
                 assyr.focus();
                 $('#spmobile').css({ 'display': 'block' });
                 returnData = false;
             }
             //if (CchkResPersaddr.GetChecked() == false) {

             //    $('#spResPersaddr').css({ 'display': 'block' });
             //    returnData = false;
             //}

             var panVal = $('#txtRePanPers').val();
             var regpan = /^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$/;
             if ($('#txtRePanPers').val() != "") {
                 if (!regpan.test(panVal)) {
                     alert("Invaild PAN Card No."); // valid pan card number
                     returnData = false;
                 }
             }
             var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;


             if ($("#txtDeductEmail").val() != "") {
                 if (reg.test($("#txtDeductEmail").val()) == false) {
                     alert('Invalid Email Address');
                     returnData = false;
                 }
             }

             if ($("#txtPersemail").val() != "") {
                 if (reg.test($("#txtPersemail").val()) == false) {
                     alert('Invalid Email Address');
                     returnData = false;
                 }
             }


             if ($('#txtRePanPers').val() == "") {
                 var assyr = document.getElementById('<%= txtRePanPers.ClientID %>');
                 assyr.focus();
                 $('#spRePanPers').css({ 'display': 'block' });
                 returnData = false;
             }
             return returnData;
        }
        //another function
        function DeductorPinChange() {

            var BBSPin = ctxtDeductpin.GetText();
            if ((BBSPin.length > 0) && (BBSPin.length <= 20)) {
                CustomerDetailsByPin();
            }
        }
        //another function
        function CustomerDetailsByPin()
        {
             var detailsByPin = ctxtDeductpin.GetText().trim();
            if (detailsByPin != '')
            {
                //var details = {}
                //details.PinCode = detailsByPin;
                $.ajax({
                    type: "POST",
                    url: "UserControls/TDSdeduction.asmx/CustomAddressByPin",
                    data: JSON.stringify({ PinCode: detailsByPin }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg)
                    {
                        var obj = msg.d;
                        var returnObj = obj[0];
                        //Billing HdndeductPin
                        if (returnObj)
                        { ctxtDeductpin.SetText(returnObj.PinCode);
                            $('#HdndeductPin').val(returnObj.PinId);
                            CtxtDeductState.SetText(returnObj.StateName);
                            $('#hdnDeductStateid').val(returnObj.StateId);
                            $('#hdnDeductStateCode').val(returnObj.StateCode);
                         }
                        else
                        {
                            ctxtDeductpin.SetText("");
                            $('#HdndeductPin').val("");
                            CtxtDeductState.SetText("");
                            $('#hdnDeductStateid').val("");
                            $('#hdnDeductStateCode').val("");
                        }
                    }
                });
            }
        }
        //another function
        function DeductorPersonPinChange() {

            var BBSPin = CtxtpersPin.GetText();
            if ((BBSPin.length > 0) && (BBSPin.length <= 20)) {
                PersonsDetailsByPin();
            }
        }
        function PersonsDetailsByPin() {

            var detailsByPin = CtxtpersPin.GetText().trim();
            if (detailsByPin != '') {
                var details = {}

                details.PinCode = detailsByPin;
                $.ajax({
                    type: "POST",
                    url: "UserControls/TDSdeduction.asmx/CustomAddressByPin",
                    data: JSON.stringify({ PinCode: detailsByPin }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var obj = msg.d;
                        var returnObj = obj[0];

                       // Billing HdndeductPin;
                        if (returnObj) {

                            CtxtpersPin.SetText(returnObj.PinCode);
                            $('#hdnPersPinId').val(returnObj.PinId);
                            CtxtPersState.SetText(returnObj.StateName);
                            $('#hdnPersStateId').val(returnObj.StateId);
                            $('#hdnPersStateCode').val(returnObj.StateCode);

                        }
                        else
                        {
                            CtxtpersPin.SetText("");
                            $('#hdnPersPinId').val("");
                            CtxtPersState.SetText("");
                            $('#hdnPersStateId').val("");
                            $('#hdnPersStateCode').val("");
                        }
                    }
                });
            }
        }
        //end of srijeeta mantis issue 0024438

        var pinCodeWithAreaId = [];
        $(document).ready(function () {
            ListBind();
            /*Code  Added  By Priti on 06122016 to use jquery Choosen for BranchHead*/
            ChangeSourceBranchHead();
            //.............end........
           
            var cntry = document.getElementById('txtCountry_hidden').value;
            document.getElementById('txtCountry_hidden').value = "";

            //var Statery = document.getElementById('txtState_hidden').value;
            //    document.getElementById('txtState_hidden').value = "";

            // var cityry = document.getElementById('txtCity_hidden').value;
            //    document.getElementById('txtCity_hidden').value = "";
            setCountry(cntry);
            //setState(Statery);
            //setCity(cityry);
            setMainAccount(document.getElementById('hdlstMainAccount').value);
        });

        function ClientSaveClick() {
            var returnValue = true;
            document.getElementById('txtCountry_hidden').value = document.getElementById('lstCountry').value;
            document.getElementById('txtState_hidden').value = document.getElementById('lstState').value;
            document.getElementById('txtCity_hidden').value = document.getElementById('lstCity').value;
            document.getElementById('hdLstArea').value = document.getElementById('lstArea').value;
            document.getElementById('HdPin').value = document.getElementById('lstPin').value;
            /*Code  Added  By Priti on 06122016 to use jquery Choosen for BranchHead*/
            document.getElementById('txtBranchHead_hidden').value = document.getElementById('lstBranchHead').value;

            returnValue = validateControl();

            return returnValue;

        }


        function validateControl() {
            debugger;
            var isValid = true;
            Page_ClientValidate();
            $('#invalidGst').css({ 'display': 'none' });
            var gst1 = ctxtGSTIN1.GetText().trim();
            var gst2 = ctxtGSTIN2.GetText().trim();
            var gst3 = ctxtGSTIN3.GetText().trim();

            if (gst1.length == 0 && gst2.length == 0 && gst3.length == 0) {
                isValid = true;
            }
            else {
                if (gst1.length != 2 || gst2.length != 10 || gst3.length != 3) {
                    $('#invalidGst').css({ 'display': 'block' });
                    isValid = false;
                }


                var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
                var code = /([C,P,H,F,A,T,B,L,J,G])/;
                var code_chk = gst2.substring(3, 4);
                if (gst2.search(panPat) == -1) {
                    $('#invalidGst').css({ 'display': 'block' });
                    isValid = false;
                }
                if (code.test(code_chk) == false) {
                    $('#invalidGst').css({ 'display': 'block' });
                    isValid = false;
                }
            }


            //Mantis Issue 24499
            var tanVal = $('#txtlocalSalesTax').val();
            var regtan = /^([a-zA-Z]){4}([0-9]){5}([a-zA-Z]){1}?$/;
            if ($('#txtlocalSalesTax').val() != "") {
                if (!regtan.test(tanVal)) {
                    alert("Invaild TAN No.");
                    isValid = false;
                }
            }
            //End of Pratik

           //// var panPatPAN = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;


            var PAN = ctxtNumber.GetText();
            var panPatPAN = /[A-Z]{5}[0-9]{4}[A-Z]{1}$/;
            var codePAN = /([C,P,H,F,A,T,B,L,J,G])/;
            var code_chkPAN = PAN.substring(3, 4);
            if (PAN != "") {
                if (PAN.search(panPatPAN) == -1) {

                    jAlert('Please enter valid pan no.')
                    return false;
                }
                if (codePAN.test(code_chkPAN) == false) {

                    return false;
                }

            }





            return isValid;
        }


        function setCountry(obj) {
            if (obj) {
                var lstCntry = document.getElementById("lstCountry");
                for (var i = 0; i < lstCntry.options.length; i++) {
                    if (lstCntry.options[i].value == obj) {
                        lstCntry.options[i].selected = true;
                    }
                }
                $('#lstCountry').trigger("chosen:updated");
                onCountryChange();
            }
        }

        function setMainAccount(obj) {
            if (obj) {
                var lstMainAct = document.getElementById("lstMainAccount");

                for (var i = 0; i < lstMainAct.options.length; i++) {
                    if (lstMainAct.options[i].value == obj) {
                        lstMainAct.options[i].selected = true;
                    }
                }
                $('#lstMainAccount').trigger("chosen:updated");
                onlstMainAccountChange();
            }
        }


        function setState(obj) {
            if (obj) {
                var lstStae = document.getElementById("lstState");

                for (var i = 0; i < lstStae.options.length; i++) {
                    if (lstStae.options[i].value == obj) {
                        lstStae.options[i].selected = true;
                    }
                }
                $('#lstState').trigger("chosen:updated");
                onStateChange();
            }
        }
        function setCity(obj) {
            if (obj) {
                var lstCity = document.getElementById("lstCity");

                for (var i = 0; i < lstCity.options.length; i++) {
                    if (lstCity.options[i].value == obj) {
                        lstCity.options[i].selected = true;
                    }
                }
                $('#lstCity').trigger("chosen:updated");
                onCityChange();
            }
        }

        function setArea(obj) {
            if (obj) {
                var lstArea = document.getElementById("lstArea");

                for (var i = 0; i < lstArea.options.length; i++) {
                    if (lstArea.options[i].value == obj) {
                        lstArea.options[i].selected = true;
                    }
                }
                $('#lstArea').trigger("chosen:updated");

            }

        }
        function setPin(obj) {
            if (obj) {
                var lstPin = document.getElementById("lstPin");

                for (var i = 0; i < lstPin.options.length; i++) {
                    if (lstPin.options[i].value == obj) {
                        lstPin.options[i].selected = true;
                    }
                }
                $('#lstPin').trigger("chosen:updated");

            }
        }
        function onAreaChange() {
            if (document.getElementById('lstArea').value) {
                getPinCodeForArea(document.getElementById('lstArea').value);
            }
        }
        function getPinCodeForArea(obj) {

            var pinData = '';
            for (var i = 0; i < pinCodeWithAreaId.length; i++) {
                if (pinCodeWithAreaId[i].split('~')[0] == obj) {
                    console.log("pin code", pinCodeWithAreaId[i].split('~')[1]);
                    document.getElementById('txtPin').value = pinCodeWithAreaId[i].split('~')[1];
                }
            }

        }

        function onlstMainAccountChange() {
            document.getElementById('hdlstMainAccount').value = document.getElementById('lstMainAccount').value;
        }

        function onCountryChange() {
            var CountryId = "";
            if (document.getElementById('lstCountry').value) {
                CountryId = document.getElementById('lstCountry').value;
            } else {
                return;
            }
            var lState = $('select[id$=lstState]');
            var lCity = $('select[id$=lstCity]');
            var lArea = $('select[id$=lstArea]');
            var lPin = $('select[id$=lstPin]');
            lState.empty();
            lCity.empty();
            lArea.empty();
            lPin.empty();
            $('#lstCity').trigger("chosen:updated");
            $('#lstArea').trigger("chosen:updated");
            $('#lstPin').trigger("chosen:updated");
            $.ajax({
                type: "POST",
                url: "BranchAddEdit.aspx/GetStates",
                data: JSON.stringify({ CountryCode: CountryId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {
                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];

                            listItems.push('<option value="' +
                            id + '">' + name
                            + '</option>');
                        }

                        $(lState).append(listItems.join(''));

                        $('#lstState').fadeIn();
                        $('#lstState').trigger("chosen:updated");
                        if (document.getElementById('txtState_hidden').value) {
                            var stateVal = document.getElementById('txtState_hidden').value;
                            document.getElementById('txtState_hidden').value = "";
                            setState(stateVal);
                        }
                    }
                    else {
                        $('#lstState').fadeIn();
                        $('#lstState').trigger("chosen:updated");
                    }
                }
            });
        }

        function onStateChange() {
            var StateId = "";
            if (document.getElementById('lstState').value) {
                StateId = document.getElementById('lstState').value;
            }
            else {
                return;
            }
            var lCity = $('select[id$=lstCity]');
            var lArea = $('select[id$=lstArea]');
            var lPin = $('select[id$=lstPin]');
            lArea.empty();
            lCity.empty();
            lPin.empty();
            $('#lstArea').trigger("chosen:updated");
            $('#lstPin').trigger("chosen:updated");
            $.ajax({
                type: "POST",
                url: "BranchAddEdit.aspx/GetCities",
                data: JSON.stringify({ StateCode: StateId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {
                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];

                            listItems.push('<option value="' +
                            id + '">' + name
                            + '</option>');
                        }

                        $(lCity).append(listItems.join(''));

                        $('#lstCity').fadeIn();
                        $('#lstCity').trigger("chosen:updated");
                        if (document.getElementById('txtCity_hidden').value) {
                            var cityVal = document.getElementById('txtCity_hidden').value;
                            document.getElementById('txtCity_hidden').value = "";
                            setCity(cityVal);
                        }
                    }
                    else {
                        $('#lstCity').fadeIn();
                        $('#lstCity').trigger("chosen:updated");
                    }
                }
            });
        }

        function getPinList() {
            var CityId = "";
            if (document.getElementById('lstCity').value) {
                CityId = document.getElementById('lstCity').value;
            }
            else {
                return;
            }
            var lPin = $('select[id$=lstPin]');
            lPin.empty();
            $.ajax({
                type: "POST",
                url: "BranchAddEdit.aspx/GetPin",
                data: JSON.stringify({ CityCode: CityId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {
                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];

                            listItems.push('<option value="' +
                            id + '">' + name
                            + '</option>');
                        }

                        $(lPin).append(listItems.join(''));

                        $('#lstPin').fadeIn();
                        $('#lstPin').trigger("chosen:updated");
                        if (document.getElementById('HdPin').value) {
                            setPin(document.getElementById('HdPin').value);
                        }

                    }
                    else {
                        $('#lstPin').fadeIn();
                        $('#lstPin').trigger("chosen:updated");
                    }
                }
            });
        }
        function onCityChange() {
            getPinList();

            var CityId = "";
            if (document.getElementById('lstCity').value) {
                CityId = document.getElementById('lstCity').value;
            }
            else {
                return;
            }
            var lArea = $('select[id$=lstArea]');
            lArea.empty();
            pinCodeWithAreaId = [];
            $.ajax({
                type: "POST",
                url: "BranchAddEdit.aspx/GetArea",
                data: JSON.stringify({ CityCode: CityId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {
                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];
                            pin = list[i].split('|')[2];
                            listItems.push('<option value="' +
                            id + '">' + name
                            + '</option>');
                            pinCodeWithAreaId[i] = id + '~' + pin;
                        }

                        $(lArea).append(listItems.join(''));

                        $('#lstArea').fadeIn();
                        $('#lstArea').trigger("chosen:updated");
                        if (document.getElementById('hdLstArea').value) {
                            var areaVal = document.getElementById('hdLstArea').value;
                            document.getElementById('hdLstArea').value = "";
                            setArea(areaVal);
                        }
                    }
                    else {
                        $('#lstArea').fadeIn();
                        $('#lstArea').trigger("chosen:updated");
                    }
                }
            });
        }

        function ListBind() {

            var config = {
                '.chsn': {},
                '.chsn-deselect': { allow_single_deselect: true },
                '.chsn-no-single': { disable_search_threshold: 10 },
                '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
                '.chsn-width': { width: "100%" }
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }

        }
        function lstCountry() {
            $('#lstCountry').fadeIn();
        }
        /*Code  Added  By Priti on 06122016 to use jquery Choosen for BranchHead*/

        //rev Bapi
        function ValidatePanno() {
            debugger;
         
            var PAN = ctxtNumber.GetText().toUpperCase();
            var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
            var code = /([C,P,H,F,A,T,B,L,J,G])/;
            var code_chk = PAN.substring(3, 4);
            if (PAN != "") {
                if (PAN.search(panPat) == -1) {
                  

                    return false;
                }
                if (code.test(code_chk) == false) {
                 
                    return false;
                }

            }




        }

        //end rev Bapi

        function lstBranchHead() {
            $('#lstBranchHead').fadeIn();
        }

        function ChangeselectedvalueBranchHead() {
            var lstBranchHead = document.getElementById("lstBranchHead");
            if (document.getElementById("txtBranchHead_hidden").value != '') {
                for (var i = 0; i < lstBranchHead.options.length; i++) {
                    if (lstBranchHead.options[i].value == document.getElementById("txtBranchHead_hidden").value) {
                        lstBranchHead.options[i].selected = true;
                    }
                }
                $('#lstBranchHead').trigger("chosen:updated");
            }

        }
        function ChangeSourceBranchHead() {
            var fname = "%";
            var lBranchHead = $('select[id$=lstBranchHead]');
            lBranchHead.empty();

            $.ajax({
                type: "POST",
                url: "BranchAddEdit.aspx/GetBranchHead",
                data: JSON.stringify({ reqStr: fname }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                asynch: false,
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {
                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];

                            $('#lstBranchHead').append($('<option>').text(name).val(id));

                        }

                        $(lBranchHead).append(listItems.join(''));

                        lstBranchHead();
                        $('#lstBranchHead').trigger("chosen:updated");


                        ChangeselectedvalueBranchHead();
                        //AdminSelected()
                    }
                    else {
                        //// alert("No records found");
                        //lstReferedBy();
                        $('#lstBranchHead').trigger("chosen:updated");

                    }
                    //Mantis Issue 24358
                    if (document.getElementById('lstBranchHead').value == "" || document.getElementById('lstBranchHead').value == null || document.getElementById('lstBranchHead').value == undefined) {
                        var lstBHead = document.getElementById("lstBranchHead");
                        //alert(lstBHead.options.length)
                        for (var i = 0; i < lstBHead.options.length; i++) {
                            //alert(lstBHead.options[i].value)
                            if (lstBHead.options[i].value == 'EMA0000001') {
                                //alert(lstBHead.options[i].value)
                                lstBHead.options[i].selected = true;
                            }
                        }
                        $('#lstBranchHead').trigger("chosen:updated");
                    }
                    
                    //End of Mantis Issue 24358
                }
            });
            // }
            
        }
        //..............end...............
       
        function ValidatePage() {
            var BranchCode = document.getElementById("PageControl1_txtCode").value;
            if (document.getElementById("PageControl1_txtCode").value == '') {
                alert('Branch Code Required!..');
                return false;
            }
            else if (document.getElementById("PageControl1_txtBranchDesc").value == '') {
                alert('Branch Name Required!..');
                return false;
            }

            if (BranchCode.length < 3) {
                alert('Branch Code Should be 3 characters!..');
                return false;
            }


        }
        function disp_prompt(name) {
            //if (name == "tab2") {
            //    document.location.href = "Contact_Document.aspx?Page=branch";

            //}
            if (name == "tab2") {
                document.location.href = "frm_branchUdf.aspx";
            }
            else if (name == "tab3") {
                document.location.href = "Branch_Correspondance.aspx?Page=branch";
            }
            else if (name == "tab4") {
                document.location.href = "Contact_Document.aspx?Page=branch";

            }
        }

        function Close() {
            editwin.close();
        }


        function CallAjaxState(obj1, obj2, obj3) {


            if (obj1.value == "") {
                obj1.value = "%";
            }
            var obj5 = document.getElementById("txtCountry_hidden").value;
            ajax_showOptionsTEST(obj1, obj2, obj3, obj5);
            if (obj5 != '') {
                ajax_showOptions(obj1, obj2, obj3, obj5);
                if (obj1.value == "%") {
                    obj1.value = "";
                }
            }
            else {
                alert("Please Select Country!..")
            }


        }


        function CallAjaxCity(obj1, obj2, obj3) {


            if (obj1.value == "") {
                obj1.value = "%";
            }
            var obj5 = document.getElementById("txtState_hidden").value;
            if (obj5 != '') {
                ajax_showOptionsTEST(obj1, obj2, obj3, obj5);
                if (obj1.value == "%") {
                    obj1.value = "";
                }
            }
            else {
                alert("Please Select State!..")
            }
        }


        function OnEditButtonClick(keyValue) {
            var url = 'BranchAddEdit.aspx?id=' + keyValue;
            //parent.OnMoreInfoClick(url, "Edit Account", '820px', '400px', "Y");
            window.location.href = url;
        }

        function OnAddButtonClick() {
            var url = 'BranchAddEdit.aspx?id=ADD';
            //OnMoreInfoClick(url, "Add New Account", '820px', '400px', "Y");
            window.location.href = url;
        }


        function DeleteRow(keyValue) {
            doIt = confirm('Confirm delete?');
            if (doIt) {
                grid.PerformCallback('Delete~' + keyValue);
                //height();
            }
            else {

            }
        }


        function ShowHideFilter1(obj) {

            gridTerminal.PerformCallback(obj);
        }

        function OnCountryChanged(cmbCountry) {



            drpState.PerformCallback(obj);


        }
        function OnStateChanged(cmbState) {

        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function setvaluetovariable(obj1) {
            combo1.PerformCallback(obj1);
        }
        function setvaluetovariable1(obj1) {
            combo2.PerformCallback(obj1);
        }
        function CallList(obj1, obj2, obj3) {

            if (obj1.value == "") {
                obj1.value = "%";
            }
            var obj5 = '';
            ajax_showOptionsTEST(obj1, obj2, obj3, obj5);
            if (obj1.value == "%") {
                obj1.value = "";
            }
        }
        function hide_show(obj) {
            if (obj == 'All') {
                document.getElementById("client_pro").style.display = "none";
            }

        }
        function GetClick() {
            btnC.PerformCallback();
        }
        function Page_Load() {
            document.getElementById("TdCombo").style.display = "none";
        }
        function Message(obj) {
            if (obj == "update") {
                alert('Update Successfully');
                gridTerminal.PerformCallback();
            }
            else if (obj == "insert") {
                alert('Insert Successfully');
                gridTerminal.PerformCallback();
            }
            else {
                alert('Terminal Id Already Exists');
            }
        }
        function CheckingTD(obj) {

            var gridstat = gridTerminal.cpCompCombo;
            if (gridstat == 'anew')
                combo.SetFocus();


        }
        FieldName = "cmbExport_DDDWS";
        function LastCall(obj) {

        }
        // Code Added By Priti on 21122016 to check Unique Short Name
        function fn_ctxtPro_Name_TextChanged() {
            var ShortName = document.getElementById("txtCode").value;
            var qString = window.location.href.split("=")[1];
            $.ajax({
                type: "POST",
                url: "BranchAddEdit.aspx/CheckUniqueName",
                data: JSON.stringify({ ShortName: ShortName, qString: qString }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;

                    if (data == true) {
                        jAlert("Please enter unique Short Name");
                        document.getElementById("txtCode").value = '';
                        document.getElementById("txtCode").focus();
                        return false;
                    }
                }
            });
        }

    </script>
    <style type="text/css">
        .abs {
            position: absolute;
            right: -19px;
            top: 4px;
        }

        .abs1 {
            position: absolute;
            right: -19px;
            top: 4px;
        }

        .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }

        #lstCountry, #lstState, #lstCity, #lstArea, #lstPin, #lstBranchHead, #lstMainAccount {
            width: 100%;
        }

        #lstCountry, #lstState, #lstCity, #lstArea, #lstPin, #lstBranchHead, #lstMainAccount {
            display: none !important;
        }

        #lstCountry_chosen, #lstState_chosen, #lstCity_chosen, #lstArea_chosen, #lstPin_chosen, #lstBranchHead_chosen {
            width: 100% !important;
        }

        #PageControl1_CC {
            overflow: visible !important;
        }

        #lstState_chosen, #lstCountry_chosen, #lstCity_chosen, #lstPin_chosen, #lstBranchHead_chosen {
            margin-bottom: 5px;
        }

        .divControlClass > span.controlClass {
            margin-top: 8px;
        }

        .nestedinput {
            padding: 0;
            margin: 0;
        }

            .nestedinput li {
                list-style-type: none;
                display: inline-block;
                float: left;
            }

                .nestedinput li.dash {
                    width: 26px;
                    text-align: center;
                    padding: 6px;
                }

                .nestedinput li .iconRed {
                    position: absolute;
                    right: -10px;
                    top: 5px;
                }

                /*Rev 1.0*/
        .outer-div-main {
            background: #ffffff;
            padding: 10px;
            border-radius: 10px;
            box-shadow: 1px 1px 10px #11111154;
        }

        /*.form_main {
            overflow: hidden;
        }*/

        label , .mylabel1, .clsTo, .dxeBase_PlasticBlue
        {
            color: #141414 !important;
            font-size: 14px !important;
                font-weight: 500 !important;
                margin-bottom: 0 !important;
                    line-height: 20px;
        }

        #GrpSelLbl .dxeBase_PlasticBlue
        {
                line-height: 20px !important;
        }

        select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , .dxeTextBox_PlasticBlue
        {
            height: 30px;
            border-radius: 4px;
        }

        .dxeButtonEditButton_PlasticBlue
        {
            background: #094e8c !important;
            border-radius: 4px !important;
            padding: 0 4px !important;
        }

        .calendar-icon {
            position: absolute;
            bottom: 6px;
            right: 20px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #txtDOB , #txtAnniversary , #txtcstVdate , #txtLocalVdate ,
        #txtCINVdate , #txtincorporateDate , #txtErpValidFrom , #txtErpValidUpto , #txtESICValidFrom , #txtESICValidUpto
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1 , #txtDOB_B-1 , #txtAnniversary_B-1 , #txtcstVdate_B-1 ,
        #txtLocalVdate_B-1 , #txtCINVdate_B-1 , #txtincorporateDate_B-1 , #txtErpValidFrom_B-1 , #txtErpValidUpto_B-1 , #txtESICValidFrom_B-1 ,
        #txtESICValidUpto_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img ,
        #txtDOB_B-1 #txtDOB_B-1Img ,
        #txtAnniversary_B-1 #txtAnniversary_B-1Img ,
        #txtcstVdate_B-1 #txtcstVdate_B-1Img ,
        #txtLocalVdate_B-1 #txtLocalVdate_B-1Img , #txtCINVdate_B-1 #txtCINVdate_B-1Img , #txtincorporateDate_B-1 #txtincorporateDate_B-1Img ,
        #txtErpValidFrom_B-1 #txtErpValidFrom_B-1Img , #txtErpValidUpto_B-1 #txtErpValidUpto_B-1Img , #txtESICValidFrom_B-1 #txtESICValidFrom_B-1Img ,
        #txtESICValidUpto_B-1 #txtESICValidUpto_B-1Img
        {
            display: none;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        .simple-select::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 26px;
            right: 13px;
            font-size: 16px;
            transform: rotate(269deg);
            font-weight: 500;
            background: #094e8c;
            color: #fff;
            height: 18px;
            display: block;
            width: 26px;
            /* padding: 10px 0; */
            border-radius: 4px;
            text-align: center;
            line-height: 19px;
            z-index: 0;
        }
        .simple-select {
            position: relative;
        }
        .simple-select:disabled::after
        {
            background: #1111113b;
        }
        /*select.btn
        {
            padding-right: 10px !important;
        }*/

        .panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }

        .dxpLite_PlasticBlue .dxp-current
        {
            background-color: #1b5ea4;
            padding: 3px 5px;
            border-radius: 2px;
        }

        #accordion {
            margin-bottom: 20px;
            margin-top: 10px;
        }

        .dxgvHeader_PlasticBlue {
    background: #1b5ea4 !important;
    color: #fff !important;
}
        #ShowGrid
        {
            margin-top: 10px;
        }

        .pt-25{
                padding-top: 25px !important;
        }

        .styled-checkbox {
        position: absolute;
        opacity: 0;
        z-index: 1;
    }

        .styled-checkbox + label {
            position: relative;
            /*cursor: pointer;*/
            padding: 0;
            margin-bottom: 0 !important;
        }

            .styled-checkbox + label:before {
                content: "";
                margin-right: 6px;
                display: inline-block;
                vertical-align: text-top;
                width: 16px;
                height: 16px;
                /*background: #d7d7d7;*/
                margin-top: 2px;
                border-radius: 2px;
                border: 1px solid #c5c5c5;
            }

        .styled-checkbox:hover + label:before {
            background: #094e8c;
        }


        .styled-checkbox:checked + label:before {
            background: #094e8c;
        }

        .styled-checkbox:disabled + label {
            color: #b8b8b8;
            cursor: auto;
        }

            .styled-checkbox:disabled + label:before {
                box-shadow: none;
                background: #ddd;
            }

        .styled-checkbox:checked + label:after {
            content: "";
            position: absolute;
            left: 3px;
            top: 9px;
            background: white;
            width: 2px;
            height: 2px;
            box-shadow: 2px 0 0 white, 4px 0 0 white, 4px -2px 0 white, 4px -4px 0 white, 4px -6px 0 white, 4px -8px 0 white;
            transform: rotate(45deg);
        }

        .dxgvEditFormDisplayRow_PlasticBlue td.dxgv, .dxgvDataRow_PlasticBlue td.dxgv, .dxgvDataRowAlt_PlasticBlue td.dxgv, .dxgvSelectedRow_PlasticBlue td.dxgv, .dxgvFocusedRow_PlasticBlue td.dxgv
        {
            padding: 6px 6px 6px !important;
        }

        #lookupCardBank_DDD_PW-1
        {
                left: -182px !important;
        }
        .plhead a>i
        {
                top: 9px;
        }

        .clsTo
        {
            display: flex;
    align-items: flex-start;
        }

        input[type="radio"], input[type="checkbox"]
        {
            margin-right: 5px;
        }
        .dxeCalendarDay_PlasticBlue
        {
                padding: 6px 6px;
        }

        .modal-dialog
        {
            width: 50%;
        }

        .modal-header
        {
            padding: 8px 4px 8px 10px;
            background: #094e8c !important;
        }

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #EmployeeGrid , .TableMain100 #RootGrid
        {
            max-width: 98% !important;
        }

        /*div.dxtcSys > .dxtc-content > div, div.dxtcSys > .dxtc-content > div > div
        {
            width: 95% !important;
        }*/

        .btn-info
        {
                background-color: #1da8d1 !important;
                background-image: none;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .dxeButtonDisabled_PlasticBlue
        {
            background: #b5b5b5 !important;
            border-color: #b5b5b5 !important;
        }

        #ddlValTech
        {
            width: 100% !important;
            margin-bottom: 0 !important;
        }

        .dis-flex
        {
            display: flex;
            align-items: baseline;
        }

        input + label
        {
            line-height: 1;
                margin-top: 3px;
        }

        .dxtlHeader_PlasticBlue
        {
            background: #094e8c !important;
        }

        .dxeBase_PlasticBlue .dxichCellSys
        {
            padding-top: 2px !important;
        }

        .pBackDiv
        {
            border-radius: 10px;
            box-shadow: 1px 1px 10px #1111112e;
        }
        .HeaderStyle th
        {
            padding: 5px;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer
        {
            padding-top: 15px;
        }

        .pt-2
        {
            padding-top: 5px;
        }
        .pt-10
        {
            padding-top: 10px;
        }

        .pt-15
        {
            padding-top: 15px;
        }

        .pb-10
        {
            padding-bottom: 10px;
        }

        .pTop10 {
    padding-top: 20px;
}
        .custom-padd
        {
            padding-top: 4px;
    padding-bottom: 10px;
        }

        input + label
        {
                margin-right: 10px;
        }

        .btn
        {
            margin-bottom: 0;
        }

        .pl-10
        {
            padding-left: 10px;
        }

        .col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }

        .devCheck
        {
            margin-top: 5px;
        }

        .mtc-5
        {
            margin-top: 5px;
        }

        .mtc-10
        {
            margin-top: 10px;
        }

        /*select.btn
        {
           position: relative;
           z-index: 0;
        }*/

        select
        {
            margin-bottom: 0;
        }

        .form-control
        {
            background-color: transparent;
        }

        select.btn-radius {
    padding: 4px 8px 6px 11px !important;
}
        .mt-30{
            margin-top: 30px;
        }

        .panel-title h3
        {
            padding-top: 0;
            padding-bottom: 0;
        }

        .btn-radius
        {
            padding: 4px 11px !important;
            border-radius: 4px !important;
        }

        .crossBtn
        {
             right: 30px;
             top: 25px;
        }

        .mb-10
        {
            margin-bottom: 10px;
        }

        .btn-cust
        {
            background-color: #108b47 !important;
            color: #fff;
        }

        .btn-cust:hover
        {
            background-color: #097439 !important;
            color: #fff;
        }

        .gHesder
        {
            background: #1b5ca0 !important;
            color: #ffffff !important;
            padding: 6px 0 6px !important;
        }

        .close
        {
             color: #fff;
             opacity: .5;
             font-weight: 400;
        }

        .mt-24
        {
            margin-top: 24px;
        }

        .col-md-3
        {
            margin-top: 8px;
        }

        #upldBigLogo , #upldSmallLogo
        {
            width: 100%;
        }

        .chosen-container-single .chosen-single span
        {
            min-height: 30px;
            line-height: 30px;
        }

        .chosen-container-single .chosen-single div {
        background: #094e8c;
        color: #fff;
        border-radius: 4px;
        height: 26px;
        top: 1px;
        right: 1px;
        /*position:relative;*/
    }

        .chosen-container-single .chosen-single div b {
            display: none;
        }

        .chosen-container-single .chosen-single div::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 2px;
            right: 5px;
            font-size: 18px;
            transform: rotate(269deg);
            font-weight: 500;
        }

    .chosen-container-active.chosen-with-drop .chosen-single div {
        background: #094e8c;
        color: #fff;
    }

        .chosen-container-active.chosen-with-drop .chosen-single div::after {
            transform: rotate(90deg);
            right: 5px;
        }

        /*.dxeDisabled_PlasticBlue, .aspNetDisabled {
            opacity: 0.4 !important;
            color: #ffffff !important;
        }*/
                /*.padTopbutton {
            padding-top: 27px;
        }*/
        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/
    </style>
    <link href="CSS/tabLayout.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">Add/Edit Branch</h3>

            <div class="crossBtn"><a href="Branch.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
        <%--debjyoti 22-12-2016--%>
        <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
        Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

        <asp:HiddenField runat="server" ID="IsUdfpresent" />
        <asp:HiddenField runat="server" ID="Keyval_internalId" />
        <%--End debjyoti 22-12-2016--%>
        <div class="form_main">
        <table style="width: 100%">
            <tr>
                <td style="width: 100%">
                    <dxe:ASPxPageControl ID="PageControl1" runat="server" Width="100%" ActiveTabIndex="0"
                        ClientInstanceName="page">
                        <TabPages>
                            <dxe:TabPage Text="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <div class="totalWrap">
                                            <%--Rev 1.0 : "simple-select" class add --%>
                                            <div class="col-md-3 simple-select">
                                                <label>Branch Type </label>
                                                <div class="relative">
                                                    <asp:DropDownList ID="cmbBranchType" runat="server" Width="100%">
                                                        <asp:ListItem Text="Own Branch" Value="Own Branch"></asp:ListItem>
                                                        <asp:ListItem Text="Franchisee" Value="Franchisee"></asp:ListItem>
                                                        <asp:ListItem Text="Rental" Value="Rental"></asp:ListItem>
                                                        <asp:ListItem Text="Service Center" Value="ServiceCenter"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Short Name <span style="color: red">*</span></label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtCode" runat="server" ClientIDMode="Static" Width="100%"
                                                        MaxLength="80" onchange="fn_ctxtPro_Name_TextChanged()">
                                                                   
                                                    </asp:TextBox>

                                                    <asp:RequiredFieldValidator Display="Dynamic" runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtCode"
                                                        SetFocusOnError="true" ErrorMessage="" class="pullrightClass fa fa-exclamation-circle abs iconRed" ToolTip="Mandatory" ValidationGroup="branchgrp">                                                        
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <%--Rev 1.0 : "simple-select" class add --%>
                                            <div class="col-md-3 simple-select">
                                                <label>Parent Branch </label>
                                                <div class="relative">
                                                    <asp:DropDownList ID="cmbParentBranch" runat="server" Width="100%">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Branch Name <span style="color: red">*</span></label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtBranchDesc" runat="server" Width="100%" MaxLength="100"></asp:TextBox>
                                                    <asp:RequiredFieldValidator Display="Dynamic" runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtBranchDesc"
                                                        SetFocusOnError="true" ErrorMessage="" class="pullrightClass fa fa-exclamation-circle abs1 iconRed" ToolTip="Mandatory" ValidationGroup="branchgrp">                                                        
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <%--Rev 1.0 : "simple-select" class add --%>
                                            <div class="col-md-3 simple-select">
                                                <label>Region </label>
                                                <div class="relative">
                                                    <asp:DropDownList ID="cmbBranchRegion" runat="server" Width="100%">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Address1</label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtAddress1" runat="server" Width="100%" MaxLength="100"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Address2 </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtAddress2" runat="server" Width="100%" MaxLength="100"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Address3 </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtAddress3" runat="server" Width="100%" MaxLength="100"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Country <span style="color: red">*</span> </label>
                                                <div class="relative">
                                                    <%-- <asp:TextBox ID="txtCountry" runat="server" Width="250px" TabIndex="9"></asp:TextBox>--%>
                                                    <asp:ListBox ID="lstCountry" CssClass="chsn" runat="server" Width="100%" data-placeholder="Select..." onchange="onCountryChange()"></asp:ListBox>
                                                    <asp:HiddenField ID="txtCountry_hidden" runat="server" />
                                                    <asp:RequiredFieldValidator Display="Dynamic" runat="server" ID="RequiredFieldValidator5" ControlToValidate="lstCountry"
                                                        SetFocusOnError="true" ErrorMessage="" class="pullrightClass fa fa-exclamation-circle abs1 iconRed" ToolTip="Mandatory" ValidationGroup="branchgrp">                                                        
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>State <span style="color: red">*</span> </label>
                                                <div class="relative">
                                                    <%--<asp:TextBox ID="txtState" runat="server" Width="250px" TabIndex="10"></asp:TextBox>--%>
                                                    <asp:ListBox ID="lstState" CssClass="chsn" runat="server" Width="100%" data-placeholder="Select State.." onchange="onStateChange()"></asp:ListBox>
                                                    <asp:HiddenField ID="txtState_hidden" runat="server" />
                                                    <asp:RequiredFieldValidator Display="Dynamic" runat="server" ID="RequiredFieldValidator4" ControlToValidate="lstState"
                                                        SetFocusOnError="true" ErrorMessage="" class="pullrightClass fa fa-exclamation-circle abs1 iconRed" ToolTip="Mandatory" ValidationGroup="branchgrp">                                                        
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>City / District <span style="color: red">*</span> </label>
                                                <div class="relative">
                                                    <%--<asp:TextBox ID="txtCity" runat="server" Width="250px" TabIndex="11"></asp:TextBox>--%>
                                                    <asp:ListBox ID="lstCity" CssClass="chsn" runat="server" Width="100%" data-placeholder="Select City.." onchange="onCityChange()"></asp:ListBox>
                                                    <asp:HiddenField ID="txtCity_hidden" runat="server" />

                                                    <asp:RequiredFieldValidator Display="Dynamic" runat="server" ID="RequiredFieldValidator3" ControlToValidate="lstCity"
                                                        SetFocusOnError="true" ErrorMessage="" class="pullrightClass fa fa-exclamation-circle abs1 iconRed" ToolTip="Mandatory" ValidationGroup="branchgrp">                                                        
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Area </label>
                                                <div class="relative">
                                                    <%--<asp:TextBox ID="txtCity" runat="server" Width="250px" TabIndex="11"></asp:TextBox>--%>
                                                    <asp:ListBox ID="lstArea" CssClass="chsn" runat="server" Width="100%" data-placeholder="Select area.." onchange="onAreaChange()"></asp:ListBox>
                                                    <asp:HiddenField ID="hdLstArea" runat="server" />

                                                </div>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="col-md-3">
                                                <label>PIN </label>
                                                <div class="relative">
                                                    <%-- <asp:TextBox ID="txtPin" runat="server" Width="250px" TabIndex="12" MaxLength="50"></asp:TextBox>--%>
                                                    <asp:ListBox ID="lstPin" CssClass="chsn" runat="server" Width="100%" data-placeholder="Select..."></asp:ListBox>
                                                    <asp:HiddenField ID="HdPin" runat="server" />
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Branch Phone </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtPhone" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Fax </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtFax" runat="server" Width="100%" MaxLength="12"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Branch Head</label>
                                                <div class="relative">
                                                    <asp:ListBox ID="lstBranchHead" CssClass="chsn" runat="server" Width="100%" data-placeholder="Select..."></asp:ListBox>
                                                    <%-- <asp:TextBox ID="txtBranchHead" runat="server" Width="250px" TabIndex="15"></asp:TextBox>--%>
                                                    <asp:HiddenField ID="txtBranchHead_hidden" runat="server" />
                                                </div>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="col-md-3">
                                                <label>Contact Person Phone</label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtContPhone" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Contact Person</label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtContPerson" runat="server" Width="100%" MaxLength="100"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Contact Person Email</label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtContEmail" runat="server" Width="100%" MaxLength="100"></asp:TextBox>
                                                </div>
                                            </div>


                                            <div class="col-md-3">
                                                <label>Ledger Posting Account </label>
                                                <div class="relative">
                                                    <asp:ListBox ID="lstMainAccount" CssClass="chsn" runat="server" Width="100%" data-placeholder="Select..." onchange="onlstMainAccountChange()"></asp:ListBox>
                                                    <asp:HiddenField ID="hdlstMainAccount" runat="server" />
                                                </div>
                                            </div>
                                            <div class="clear"></div>
                                            <%--for Gstin--%>
                                            <div class="col-md-3">
                                                <label class="labelt">GSTIN   </label>
                                                <div class="relative">
                                                    <ul class="nestedinput">
                                                        <li>
                                                            <dxe:ASPxTextBox ID="txtGSTIN1" ClientInstanceName="ctxtGSTIN1" MaxLength="2" runat="server" Width="33px">
                                                                <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                                            </dxe:ASPxTextBox>
                                                        </li>
                                                        <li class="dash">- </li>
                                                        <li>
                                                            <dxe:ASPxTextBox ID="txtGSTIN2" ClientInstanceName="ctxtGSTIN2" MaxLength="10" runat="server" Width="90px">
                                                                <ClientSideEvents KeyUp="Gstin2TextChanged" />
                                                            </dxe:ASPxTextBox>
                                                        </li>
                                                        <li class="dash">- </li>
                                                        <li>
                                                            <dxe:ASPxTextBox ID="txtGSTIN3" ClientInstanceName="ctxtGSTIN3" MaxLength="3" runat="server" Width="50px">
                                                            </dxe:ASPxTextBox>
                                                            <span id="invalidGst" class="fa fa-exclamation-circle iconRed " style="color: red; display: none; padding-left: 9px; left: 302px;" title="Invalid GSTIN"></span>
                                                        </li>
                                                    </ul>

                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>CIN </label>
                                                <div>
                                                    <asp:TextBox ID="txtCIN" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>CIN Validity Date </label>
                                                <div>
                                                    <dxe:ASPxDateEdit ID="txtCINVdate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                        Width="100%">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>IEC Code</label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtIecCode" runat="server" Width="100%" MaxLength="10"></asp:TextBox>
                                                </div>
                                            </div>


                                            <div class="clear"></div>

                                              <div class="col-md-3">
                                                <label>MSME/Udyam RC No.</label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtMSMEUdyamRCNo" runat="server" Width="100%" MaxLength="25"></asp:TextBox>
                                                </div>
                                            </div>
                                             <%--Mantis Issue 24499--%>
                                           <div class="col-md-3">
                                                <label>TAN </label>
                                                <div>
                                                    <asp:TextBox ID="txtlocalSalesTax" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                             <div class="col-md-3">
                                                <label>Category(deductor/collector)</label>
                                                <div class="relative">
                                                    <dxe:ASPxComboBox ID="drdCategory" ClientInstanceName="cdrdCategory" Enabled="true"
                                                        runat="server" Width="100%" MaxLength="20" DataSourceID="drCategory" ValueField="deductcategory_value" TextField="deductcategory_description">
                                                    </dxe:ASPxComboBox>
                                                    
                                                    <span id="badd1" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; display: none;" title="Empty Category"></span>
                                                   
                                                    <asp:SqlDataSource runat="server" ID="drCategory" SelectCommand="SELECT [deductcategory_value], [deductcategory_description] deductcategory_description FROM [Tbl_master_deductorcategory]"></asp:SqlDataSource>
                                                </div>

                                            </div>
                                             <%--End of Mantis Issue 24499--%>
                                               <div class="col-md-2">
                                                <div id="Number" style="width:100%">
                                                    <label style="margin-top: 7px;">PAN
                                                    <a href="#"  style="left: -12px; top: 20px;"> 
                                                <i id="I1" runat="server" class="fa fa-question" aria-hidden="true"  onclick="AboutPanClick()"></i>                                              
                                                             </a>
                                                        </label>
                                                    <div >
                                                        <dxe:ASPxTextBox ID="txtNumber" ClientInstanceName="ctxtNumber" ClientEnabled="true" MaxLength="10"
                                                            runat="server" Height="30px" Width="100%">   
                                                        </dxe:ASPxTextBox>
                                                          
                                                    </div>
                                                    <dxe:ASPxLabel ID="labelformat" Style="color: #03A9F4;width: 100px;width: 100%;font-size: 11px;font-weight: 600;display: inline-block;" Width="100px" runat="server" ForeColor="#0099FF" CssClass="formatcss" ClientInstanceName="lbformat" Text="Sample : AAAAA9999A"></dxe:ASPxLabel>
                                                </div>
                                         </div>
                                             <script>
                                                 function AboutPanClick() {
                                                     $('#PanModel').modal('show');
                                                 }
                                              </script>
                                           
                                          <%--  rev Bapi

                                              <%-- Pan No Details --%>
     <div class="modal fade pmsModal w30" id="PanModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">PAN</h4>
                </div>
                <div class="modal-body">
              <div id="SalesManTable">
                  <font color="red">
                               1.First three characters are alphabetic series running from AAA to ZZZ<br />
                               2.Fourth character of PAN represents the status of the PAN holder.C — Company P — Person H — HUF(Hindu Undivided Family) <br />
                               3.F — Firm A — Association of Persons (AOP) T — AOP (Trust) B — Body of Individuals (BOI) L — Local Authority J — Artificial Juridical Person G — Government<br />
                               4.Fifth character represents the first character of the PAN holder’s last name/surname.<br />
                               5.Next four characters are sequential number running from 0001 to 9999.<br />
                               6.Last character in the PAN is an alphabetic check digit.<br />
                    </font>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>
    <%-- End Pan No Details --%>
<%--                                            end rev bapi--%>

                                            <div class="clear"></div>

                                            <div class="col-md-12" style="padding-top: 10px;">
                                                <%-- <% if (rights.CanAdd || rights.CanEdit)
                                                    { %>--%>
                                                <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="branchgrp" CssClass="btn btn-primary dxbButton" OnClick="btnSave_Click" OnClientClick="return ClientSaveClick();" />
                                                <asp:Button ID="Button2" runat="server" Text="Cancel" CssClass="btn btn-danger dxbButton" OnClick="Button2_Click" />
                                                <%--  <input type="button" id="btnUdf" value="UDF" class="btn btn-primary dxbButton" onclick="OpenUdf()" />--%>

                                                <asp:Button ID="btnUdf" runat="server" Text="UDF" CssClass="btn btn-cust dxbButton" OnClientClick="if(OpenUdf()){ return false;}" />

                                                <%--  <% } %>--%>
                                            </div>
                                        </div>

                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="Trading Terminal" Visible="false">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <table>
                                            <tr>

                                                <td>
                                                    <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                                                    </dxe:ASPxGridViewExporter>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td id="Td4">
                                                    <a href="javascript:ShowHideFilter1('s1');"><span style="color: #000099; text-decoration: underline">Show Filter</span></a>
                                                </td>
                                                <td id="Td5">
                                                    <a href="javascript:ShowHideFilter1('All1');"><span style="color: #000099; text-decoration: underline">All Records</span></a>
                                                </td>
                                                <td class="gridcellright">
                                                    <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" BackColor="Navy"
                                                        Font-Bold="False" ForeColor="White" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                                        ValueType="System.Int32" Width="130px">
                                                        <Items>
                                                            <%--  <dxe:ListEditItem Text="Select" Value="0" />--%>
                                                            <%-- <dxe:ListEditItem Text="PDF" Value="1" />--%>
                                                            <dxe:ListEditItem Text="XLS" Value="2" />
                                                            <%--<dxe:ListEditItem Text="RTF" Value="3" />
                            <dxe:ListEditItem Text="CSV" Value="4" />--%>
                                                        </Items>
                                                        <ButtonStyle BackColor="#C0C0FF" ForeColor="Black">
                                                        </ButtonStyle>
                                                        <ItemStyle BackColor="Navy" ForeColor="White">
                                                            <HoverStyle BackColor="#8080FF" ForeColor="White">
                                                            </HoverStyle>
                                                        </ItemStyle>
                                                        <Border BorderColor="White" />
                                                        <DropDownButton Text="Export">
                                                        </DropDownButton>
                                                    </dxe:ASPxComboBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <dxe:ASPxGridView ID="gridTerminalId" ClientInstanceName="gridTerminal" KeyFieldName="TradingTerminal_ID"
                                            runat="server" DataSourceID="TrdTerminal" Width="100%" OnHtmlEditFormCreated="gridTerminalId_HtmlEditFormCreated"
                                            OnStartRowEditing="gridTerminalId_StartRowEditing" AutoGenerateColumns="False"
                                            OnCustomCallback="gridTerminalId_CustomCallback" OnCellEditorInitialize="gridTerminalId_CellEditorInitialize"
                                            OnInitNewRow="gridTerminalId_InitNewRow" OnCustomJSProperties="gridTerminalId_CustomJSProperties">

                                            <Settings ShowGroupPanel="True" ShowFooter="false" ShowStatusBar="Visible" ShowTitlePanel="false" />
                                            <SettingsEditing PopupEditFormHeight="300px" PopupEditFormHorizontalAlign="Center"
                                                PopupEditFormModal="True" PopupEditFormVerticalAlign="BottomSides" PopupEditFormWidth="800px"
                                                EditFormColumnCount="1" Mode="PopupEditForm" />
                                            <SettingsText PopupEditFormCaption="Add/Modify Branch" ConfirmDelete="Are you sure to Delete this Record!" />
                                            <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" AllowFocusedRow="True" />
                                            <Columns>
                                                <dxe:GridViewDataTextColumn FieldName="TradingTerminal_ID" ReadOnly="True" Visible="False"
                                                    VisibleIndex="0">
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormCaptionStyle Wrap="False">
                                                    </EditFormCaptionStyle>
                                                    <EditFormSettings Visible="False" />
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Exchange" ReadOnly="True" VisibleIndex="0">
                                                    <EditFormCaptionStyle Wrap="False">
                                                    </EditFormCaptionStyle>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False" />
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="TradingTerminal_TerminalID" Caption="TerminalID"
                                                    ReadOnly="True" VisibleIndex="1">
                                                    <EditFormCaptionStyle Wrap="False">
                                                    </EditFormCaptionStyle>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False" />
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="TradingTerminal_ParentTerminalID" Caption="Parent TerminalID"
                                                    ReadOnly="True" VisibleIndex="2">
                                                    <EditFormCaptionStyle Wrap="False">
                                                    </EditFormCaptionStyle>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False" />
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="AllTradeID" Caption="All Trade Name" ReadOnly="True"
                                                    VisibleIndex="3">
                                                    <EditFormCaptionStyle Wrap="False">
                                                    </EditFormCaptionStyle>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False" />
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="CliTradeID" Caption="Client Trade Name"
                                                    ReadOnly="True" VisibleIndex="4">
                                                    <EditFormCaptionStyle Wrap="False">
                                                    </EditFormCaptionStyle>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False" />
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="ProTradeID" Caption="Pro Trade Name" ReadOnly="True"
                                                    VisibleIndex="5">
                                                    <EditFormCaptionStyle Wrap="False">
                                                    </EditFormCaptionStyle>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False" />
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="brokid" Caption="Broker Name" ReadOnly="True"
                                                    VisibleIndex="6">
                                                    <EditFormCaptionStyle Wrap="False">
                                                    </EditFormCaptionStyle>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False" />
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewCommandColumn VisibleIndex="7" ShowClearFilterButton="true" ShowDeleteButton="true" ShowEditButton="true">
                                                    <%--<ClearFilterButton Visible="True">
                                                    </ClearFilterButton>
                                                    <DeleteButton Visible="True">
                                                    </DeleteButton>
                                                    <EditButton Visible="True">
                                                    </EditButton>---%>
                                                    <HeaderTemplate>
                                                        <a href="javascript:void(0);" onclick="gridTerminal.AddNewRow()"><span style="color: #000099; text-decoration: underline">Add New</span></a>
                                                    </HeaderTemplate>
                                                </dxe:GridViewCommandColumn>
                                            </Columns>
                                            <SettingsCommandButton>
                                                <ClearFilterButton Text="ClearFilter">
                                                </ClearFilterButton>

                                                <EditButton Image-Url="/assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit">
                                                </EditButton>
                                                <DeleteButton Image-Url="/assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                </DeleteButton>
                                            </SettingsCommandButton>
                                            <Templates>
                                                <EditForm>
                                                    <table width="100%">
                                                        <tr>
                                                            <td style="text-align: right">
                                                                <span class="Ecoheadtxt" style="color: Black">Company Name :</span>
                                                            </td>
                                                            <td class="gridcellleft">
                                                                <dxe:ASPxComboBox ID="comboCompany" runat="server" EnableSynchronization="False"
                                                                    EnableIncrementalFiltering="True" DataSourceID="sqlCompany" TextField="cmp_name"
                                                                    ValueField="cmp_internalId" ClientInstanceName="combo" Width="300px" ValueType="System.String">
                                                                    <ClientSideEvents ValueChanged="function(s,e){
                                                                                                                var indexr = s.GetSelectedIndex();
                                                                                                                setvaluetovariable(indexr)
                                                                                                                }" />
                                                                    <ButtonStyle Width="13px">
                                                                    </ButtonStyle>
                                                                </dxe:ASPxComboBox>
                                                            </td>
                                                            <td style="text-align: right">
                                                                <span class="Ecoheadtxt" style="color: Black">Exchange :</span>
                                                            </td>
                                                            <td class="gridcellleft">
                                                                <dxe:ASPxComboBox ID="comboExchange" runat="server" DataSourceID="SqlExchange" TextField="Exchange"
                                                                    EnableSynchronization="False" EnableIncrementalFiltering="True" ValueField="exch_internalId"
                                                                    ValueType="System.String" Width="300px" ClientInstanceName="combo1"
                                                                    OnCallback="comboExchange_Callback">
                                                                    <ClientSideEvents ValueChanged="function(s,e){
                                                                                                                var indexr = s.GetSelectedIndex();
                                                                                                                setvaluetovariable1(indexr)
                                                                                                                }" />
                                                                    <ButtonStyle Width="13px">
                                                                    </ButtonStyle>
                                                                </dxe:ASPxComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">
                                                                <span class="Ecoheadtxt" style="color: Black">CTCL Vender ID:</span>
                                                            </td>
                                                            <td class="gridcellleft">
                                                                <dxe:ASPxComboBox ID="comboVendor" runat="server" DataSourceID="SqlVendor" TextField="CTCLVendor_Name"
                                                                    ValueField="CTCLVendor_ID" ValueType="System.String" Width="300px"
                                                                    ClientInstanceName="combo2" EnableSynchronization="False" EnableIncrementalFiltering="True"
                                                                    OnCallback="comboVendor_Callback">
                                                                    <ButtonStyle Width="13px">
                                                                    </ButtonStyle>
                                                                </dxe:ASPxComboBox>
                                                            </td>
                                                            <td style="text-align: right">
                                                                <span class="Ecoheadtxt" style="color: Black">Broker:</span>
                                                            </td>
                                                            <td class="gridcellleft">
                                                                <asp:TextBox ID="txtBrokername" runat="server" CssClass="EcoheadCon" Width="300px"></asp:TextBox>
                                                            </td>
                                                            <td class="gridcellleft" style="display: none">
                                                                <asp:TextBox ID="txtMappinID" runat="server" CssClass="EcoheadCon" Width="300px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">
                                                                <span class="Ecoheadtxt" style="color: Black">Terminal Id:</span>
                                                            </td>
                                                            <td class="gridcellleft">
                                                                <asp:TextBox ID="txtTerminalId" runat="server" MaxLength="20" CssClass="EcoheadCon"
                                                                    Width="300px"></asp:TextBox>
                                                            </td>
                                                            <td style="text-align: right">
                                                                <span class="Ecoheadtxt" style="color: Black">Parent TerminalId:</span>
                                                            </td>
                                                            <td class="gridcellleft">
                                                                <dxe:ASPxComboBox ID="parentTerID" runat="server" DataSourceID="SqlParentTerminal"
                                                                    TextField="TradingTerminal_TerminalID" ValueField="TradingTerminal_TerminalID"
                                                                    EnableSynchronization="False" EnableIncrementalFiltering="True"
                                                                    Width="300px" ValueType="System.String">
                                                                    <ButtonStyle Width="13px">
                                                                    </ButtonStyle>
                                                                </dxe:ASPxComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">
                                                                <span class="Ecoheadtxt" style="color: Black">Contact Name:</span>
                                                            </td>
                                                            <td class="gridcellleft">
                                                                <asp:TextBox ID="txtContactName" runat="server" CssClass="EcoheadCon" Width="300px"></asp:TextBox>
                                                            </td>
                                                            <td style="text-align: right">
                                                                <span class="Ecoheadtxt" style="color: Black">CTCL ID:</span>
                                                            </td>
                                                            <td class="gridcellleft">
                                                                <asp:TextBox ID="txtCTCLID" runat="server" CssClass="EcoheadCon" Width="300px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <%--<tr>
                                                                                    <td style="text-align: right">
                                                                                        <span class="Ecoheadtxt" style="color: Black">Broker:</span>
                                                                                    </td>
                                                                                    <td class="gridcellleft">
                                                                                        <asp:TextBox ID="txtBrokername" runat="server" CssClass="EcoheadCon" Width="300px" ></asp:TextBox>
                                                                                    </td>
                                                                                    <td style="text-align: right">
                                                                                        <span class="Ecoheadtxt" style="color: Black"></span>
                                                                                    </td>
                                                                                    <td class="gridcellleft">
                                                                                        
                                                                                    </td>
                                                                                </tr>--%>
                                                        <tr>
                                                            <td style="text-align: right">
                                                                <span class="Ecoheadtxt" style="color: Black">Connection Mode:</span>
                                                            </td>
                                                            <td class="gridcellleft">
                                                                <asp:TextBox ID="txtConnection" runat="server" CssClass="EcoheadCon" Width="300px"></asp:TextBox>
                                                            </td>
                                                            <td style="text-align: right">
                                                                <span class="Ecoheadtxt" style="color: Black">Activation Date:</span>
                                                            </td>
                                                            <td class="gridcellleft">
                                                                <dxe:ASPxDateEdit ID="dtActivation" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                                    Width="300px">
                                                                    <ButtonStyle Width="13px">
                                                                    </ButtonStyle>
                                                                </dxe:ASPxDateEdit>
                                                                <%--  <dxe:ASPxDateEdit ID="dtActivation" runat="server" Font-Size="12px" Width="200px"
                                                                                            EditFormat="Custom" EditFormatString="dd-mm-yyyy" UseMaskBehavior="True">
                                                                                            <ButtonStyle Width="13px">
                                                                                            </ButtonStyle>
                                                                                        </dxe:ASPxDateEdit>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">
                                                                <span class="Ecoheadtxt" style="color: Black">Deactivation Date:</span>
                                                            </td>
                                                            <td class="gridcellleft">
                                                                <dxe:ASPxDateEdit ID="dtDeactivation" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                                    Width="300px">
                                                                    <ButtonStyle Width="13px">
                                                                    </ButtonStyle>
                                                                </dxe:ASPxDateEdit>
                                                                <%--  <dxe:ASPxDateEdit ID="dtDeactivation" runat="server" Font-Size="12px" Width="200px"
                                                                                            EditFormat="Custom" EditFormatString="dd-mm-yyyy" UseMaskBehavior="True">
                                                                                            <ButtonStyle Width="13px">
                                                                                            </ButtonStyle>
                                                                                        </dxe:ASPxDateEdit>--%>
                                                            </td>
                                                            <td style="text-align: right" id="all1">
                                                                <span class="Ecoheadtxt" style="color: Black">All Trade Name:</span>
                                                            </td>
                                                            <td class="gridcellleft" id="all2">
                                                                <asp:TextBox ID="txtAllTrade" runat="server" CssClass="EcoheadCon" Width="300px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <%--<tr>
                                                                                    <td style="text-align: right">
                                                                                        <span class="Ecoheadtxt" style="color: Black">Broker:</span>
                                                                                    </td>
                                                                                    <td class="gridcellleft">
                                                                                        <asp:TextBox ID="txtBrokername" runat="server" CssClass="EcoheadCon" Width="300px" ></asp:TextBox>
                                                                                    </td>
                                                                                    <td class="gridcellleft" style="display:none">
                                                                                        <asp:TextBox ID="txtMappinID" runat="server" CssClass="EcoheadCon" Width="300px" ></asp:TextBox>
                                                                                    </td>
                                                                                    <td style="text-align: right" id="all1">
                                                                                        <span class="Ecoheadtxt" style="color: Black">All Trade Name:</span>
                                                                                    </td>
                                                                                    <td class="gridcellleft" id="all2">
                                                                                        <asp:TextBox ID="txtAllTrade" runat="server" CssClass="EcoheadCon" Width="300px" ></asp:TextBox>
                                                                                    </td>
                                                                                </tr>--%>
                                                        <tr id="client_pro">
                                                            <td style="text-align: right" id="client1">
                                                                <span class="Ecoheadtxt" style="color: Black">Client Trade Name:</span>
                                                            </td>
                                                            <td class="gridcellleft" id="client2">
                                                                <asp:TextBox ID="txtClientTrade" runat="server" CssClass="EcoheadCon" Width="300px"></asp:TextBox>
                                                            </td>
                                                            <td style="text-align: right" id="pro1">
                                                                <span class="Ecoheadtxt" style="color: Black">Pro Trade Name:</span>
                                                            </td>
                                                            <td class="gridcellleft" id="pro2">
                                                                <asp:TextBox ID="txtProductTrade" runat="server" CssClass="EcoheadCon" Width="300px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3" style="text-align: right">
                                                                <input id="Button1" type="button" value="Save" onclick="GetClick()" class="btnUpdate"
                                                                    style="width: 88px; height: 28px" />
                                                            </td>
                                                            <td style="text-align: left;" colspan="1">
                                                                <dxe:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel data"
                                                                    Height="18px" Width="88px" AutoPostBack="False">
                                                                    <ClientSideEvents Click="function(s, e) {gridTerminal.CancelEdit();}" />
                                                                </dxe:ASPxButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" style="text-align: right; display: none" id="TdCombo">
                                                                <dxe:ASPxComboBox ID="ASPxComboBox1" runat="server" ClientInstanceName="btnC" OnCallback="ASPxComboBox1_Callback"
                                                                    ValueType="System.String" BackColor="#C1D7F8" ForeColor="#C1D7F8">
                                                                    <Border BorderColor="#C1D7F8" />
                                                                    <ButtonStyle BackColor="#C1D7F8" ForeColor="#C1D7F8">
                                                                        <BorderBottom BorderColor="#C1D7F8" BorderStyle="None" />
                                                                        <Border BorderColor="#C1D7F8" BorderStyle="None" />
                                                                        <BorderLeft BorderStyle="None" />
                                                                        <DisabledStyle BackColor="#C1D7F8">
                                                                        </DisabledStyle>
                                                                    </ButtonStyle>
                                                                    <DropDownButton Visible="False">
                                                                    </DropDownButton>
                                                                    <ClientSideEvents EndCallback="function(s, e) {Message(btnC.cpDataExists);}" />
                                                                </dxe:ASPxComboBox>
                                                            </td>
                                                            <td style="display: none">
                                                                <asp:TextBox ID="txtProductTrade_hidden" runat="server"></asp:TextBox>
                                                                <asp:TextBox ID="txtClientTrade_hidden" runat="server"></asp:TextBox>
                                                                <asp:TextBox ID="txtAllTrade_hidden" runat="server"></asp:TextBox>
                                                                <asp:TextBox ID="txtContactName_hidden" runat="server"></asp:TextBox>
                                                                <asp:TextBox ID="txtBrokername_hidden" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EditForm>
                                            </Templates>
                                            <ClientSideEvents EndCallback="function(s,e){CheckingTD(gridTerminal.cpExist);}" />
                                        </dxe:ASPxGridView>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>


                            <dxe:TabPage Name="UDF" Text="UDF" Visible="false">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Correspondence" Text="Correspondence">
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
                            
                              <%--rev srijeeta mantis issue 0024438--%>
                          
                                
                            <dxe:TabPage Name="Deductor Info(TDS)" Text="Deductor Info(TDS)">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                     <div class="boxLayout clearfix">
                                                 <div class="hdBoxLayout">DEDUCTOR(s) INFORMATION</div>
                                                 <div class="col-md-3">
                                                <label>Financial Year<span style="color:red;"> *</span> </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtfinyr" runat="server" Width="100%" MaxLength="6"></asp:TextBox>
                                                    <asp:Label ID="lblfinyr" runat="server" style="color: blue">Value should be 201920 for Financial Year 2019-2020 </asp:Label>
                                                     <span id="spFinyr" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                </div>
                                              </div>
                                        <div class="col-md-3">
                                                <label>Assessment Year<span style="color:red;"> *</span> </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtAssyear"  runat="server" Width="100%" MaxLength="6"></asp:TextBox>

                                                    <asp:Label ID="lblasstyr" runat="server" style="color: blue">Value should be 201920 for Assessment Year 2019-2020</asp:Label>
                                                     <span id="spAssyr" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                </div>
                                              </div>

                                                 <div class="col-md-3">
                                                <label>Name of Deductor<span style="color:red;"> *</span>  </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtNamedeductor" runat="server" Width="100%" MaxLength="75"></asp:TextBox>  <%--Rev Maynak Changes max length to 75 0021268--%>
                                                     <span id="spNamedeductor" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                </div>
                                              </div>


                                               <div class="col-md-3">
                                                <label>Deductor's Branch <span style="color:red;"> *</span> </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtBranchdeduct" runat="server" Width="100%" MaxLength="75"></asp:TextBox> <%--Rev Maynak Changes max length to 75 0021268--%>
                                                    <span id="spBranchdeduct" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                </div>
                                              </div>
                                                <div class="clear"></div>

                                                  <div class="col-md-3">
                                                <label>Deductors Address1<span style="color:red;"> *</span> </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtDeductaddr1" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                     <span id="spDeductaddr1" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                </div>
                                              </div>


                                                   <div class="col-md-3">
                                                <label>Deductors Address2</label>
                                                <div>
                                                    <asp:TextBox ID="txtDeductaddr2" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                              </div>


                                                   <div class="col-md-3">
                                                <label>Deductors Address3</label>
                                                <div>
                                                    <asp:TextBox ID="txtDeductaddr3" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                              </div>


                                                   <div class="col-md-3">
                                                <label>Deductors Address4</label>
                                                <div>
                                                    <asp:TextBox ID="txtDeductaddr4" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                              </div>
                                                <div class="clear"></div>

                                                   <div class="col-md-3">
                                                <label>Deductors Address5</label>
                                                <div>
                                                    <asp:TextBox ID="txtDeductaddr5" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                              </div>
                                                <div class="col-md-3">
                                                <label>Deductors Address - PINCODE<span style="color:red;"> *</span> </label>
                                                <div class="relative">
                                                    <dxe:ASPxTextBox ID="txtDeductpin" ClientInstanceName="ctxtDeductpin" Enabled="true" runat="server" Width="100%" MaxLength="6">
                                                        <ClientSideEvents LostFocus="DeductorPinChange" />
                                                           </dxe:ASPxTextBox>
                                                    <asp:HiddenField ID="HdndeductPin" runat="server" />
                                                    <span id="spDeductpin" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                  <%--  <asp:SqlDataSource runat="server" ID="drddepin" SelectCommand="select pin_code Pincode,pin_id pinId from tbl_master_pinzip"></asp:SqlDataSource>--%>

                                                </div>
                                              </div>

                                        <div class="col-md-3">
                                                <label>Deductors Address - State<span style="color:red;"> *</span> </label>
                                                <div class="relative">
                                                  <dxe:ASPxTextBox ID="txtDeductState" ClientInstanceName="CtxtDeductState" runat="server" Width="100%" MaxLength="50" ClientEnabled="false"></dxe:ASPxTextBox>
                                                    <asp:HiddenField ID="hdnDeductStateCode" runat="server" />
                                                    <asp:HiddenField ID="hdnDeductStateid" runat="server" />
                                                    <span id="spDeductState" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                </div>
                                              </div>

                                          <%-- <div class="col-md-3">
                                                <label>Deductors Address - State</label>
                                                <div>
                                                     <dxe:ASPxComboBox ID="drdDeductState" ClientInstanceName="cdrdDeductState" Enabled="true"
                                                          runat="server" Width="100%" MaxLength="20" DataSourceID="drddeState" ValueField="stateId" TextField="statename">
                                                           </dxe:ASPxComboBox>
                                                    <asp:SqlDataSource runat="server" ID="drddeState" SelectCommand="select id stateId,state statename  from tbl_master_state"></asp:SqlDataSource>
                                                </div>
                                              </div>--%>
                                                   <div class="col-md-3">
                                                <label>Deductors EMAIL ID<span style="color:red;"> *</span> </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtDeductEmail" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                    <span id="spDedEmail" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                </div>
                                              </div>
                                        <div class="clear"></div>

                                                   <div class="col-md-3">
                                                <label>Deductors STD Code</label>
                                                <div>
                                                    <asp:TextBox ID="txtdeductSTD" runat="server" Width="100%" MaxLength="5">
                                                     </asp:TextBox>
                                                </div>
                                              </div>


                                                   <div class="col-md-3">
                                                <label>Deductors Tel-Phone No</label>
                                                <div>
                                                               <asp:TextBox ID="txtDeductTelNo" runat="server" Width="100%" MaxLength="10"></asp:TextBox>
                                                </div>
                                              </div>


                                                   <div class="col-md-6">
                                                       <div class="checkbox relative" style="padding-left: 0px !important;padding-top: 16px;">
                                                              <table>
                                                                  <tr>
                                                                      <td><dxe:ASPxCheckBox ID="ChkdeductAddrReturn"  runat="server" ClientInstanceName="cChkdeductAddrReturn"></dxe:ASPxCheckBox></td>
                                                                      <td> <dxe:ASPxLabel ID="lbldeductAddrReturn" Width="100%" runat="server" Text="Change of Address of Deductor since last Return">
                                                              </dxe:ASPxLabel>
                                                                           <span id="spdeductAddrReturn" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                                      </td>
                                                                  </tr>
                                                              </table>

                                                                        
                                                            </div>
                                                    </div>
                                                        </div>
                                         
                                            <div class="boxLayout clearfix">
                                                 <div class="hdBoxLayout">DEDUCTOR RESPONSIBLE PERSON(S) INFORMATION</div>
                                                 <div class="col-md-3">
                                                    <label>Name of Person responsible for Deduction<span style="color:red;"> *</span> </label>
                                                    <div class="relative">
                                                        <asp:TextBox ID="txtResponsibleDeduct" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                         <span id="spResponsibleDeduct" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                    </div>
                                                  </div>
                                                
                                                 <div class="col-md-3">
                                                    <label>Designation of the Person responsible for Deduction<span style="color:red;"> *</span> </label>
                                                    <div class="relative">
                                                        <asp:TextBox ID="txtdeductdesig" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                         <span id="spdeductdesig" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                    </div>
                                                  </div>

 
                                                      <div class="col-md-3">
                                                        <label>Responsible Person's Address1<span style="color:red;"> *</span> </label>
                                                        <div class="relative">
                                                            <asp:TextBox ID="txtPersaddr1" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                                <span id="spPersaddr1" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                        </div>
                                                        </div>

                                                      <div class="col-md-3">
                                                            <label>Responsible Person's Address2</label>
                                                            <div>
                                                                <asp:TextBox ID="txtPersaddr2" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                            </div>
                                                      </div>
                                                <div class="clear"></div>
                                                      <div class="col-md-3">
                                                        <label>Responsible Person's Address3</label>
                                                        <div>
                                                            <asp:TextBox ID="txtPersaddr3" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                        </div>
                                                      </div>

                                                      <div class="col-md-3">
                                                <label>Responsible Person's Address4</label>
                                                <div>
                                                    <asp:TextBox ID="txtPersaddr4" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                              </div>

                                                      <div class="col-md-3">
                                                <label>Responsible Person's Address5</label>
                                                <div>
                                                    <asp:TextBox ID="txtPersaddr5" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                              </div>

                                          <div class="col-md-3">
                                                <label>Responsible Person's PIN<span style="color:red;"> *</span> </label>
                                                <div class="relative">

                                                    <dxe:ASPxTextBox ID="txtpersPin" ClientInstanceName="CtxtpersPin" Enabled="true" runat="server" Width="100%" MaxLength="6">
                                                        <ClientSideEvents LostFocus="DeductorPersonPinChange" />
                                                           </dxe:ASPxTextBox>
                                                    <span id="spPersPin" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                    <asp:HiddenField ID="hdnPersPinId" runat="server" />

                                                    <%--<asp:SqlDataSource runat="server" ID="PersPin" SelectCommand="select pin_code RespPincode,pin_id RespPinId from tbl_master_pinzip"></asp:SqlDataSource>--%>
                                                </div>
                                              </div>
                                            
                                        <div class="clear"></div>

                                             <div class="col-md-3">
                                                <label>Responsible Person's State<span style="color:red;"> *</span> </label>
                                                <div class="relative">

                                                    <dxe:ASPxTextBox ID="txtPersState" ClientInstanceName="CtxtPersState" runat="server" Width="100%" MaxLength="50" ClientEnabled="false"></dxe:ASPxTextBox>
                                                    <asp:HiddenField ID="hdnPersStateId" runat="server" />
                                                    <asp:HiddenField ID="hdnPersStateCode" runat="server" />
                                                    <span id="spPersState" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>

                                                <%--    <dxe:ASPxComboBox ID="drdPersState" ClientInstanceName="cdrdPersState" Enabled="true"
                                                          runat="server" Width="100%" MaxLength="20" DataSourceID="persState"  ValueField="RespstateId" TextField="Respstatename">
                                                           </dxe:ASPxComboBox>
                                                    <asp:SqlDataSource runat="server" ID="persState" SelectCommand="select id RespstateId,state Respstatename  from tbl_master_state"></asp:SqlDataSource>--%>
                                                </div>
                                              </div>
                                        
                                                  

                                              <div class="col-md-3">
                                                <label>Responsible Person's Email ID<span style="color:red;"> *</span> </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtPersemail" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                    <span id="spPersEmail" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                </div>
                                              </div>

                                             <div class="col-md-3">
                                                <label>Mobile number<span style="color:red;"> *</span> </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtMobile" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                    <span id="spmobile" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                </div>
                                              </div>

                                                    <div class="col-md-3">
                                                <label>Responsible Person's STD CODE</label>
                                                <div>
                                                    <asp:TextBox ID="txtPersSTD" runat="server" Width="100%" MaxLength="5"></asp:TextBox>
                                                </div>
                                              </div>
                                                
                                                <div class="clear"></div>
                                                <div class="col-md-6">
                                                    <label>Responsible Person's Tel-Phone No:</label>
                                                    <div>
                                                        <asp:TextBox ID="txtPersTel" runat="server" Width="100%" MaxLength="10"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                <div class="checkbox relative" style="padding-left: 0px !important;padding-top: 9px;">
                                                              <table style="width:100%">
                                                                  <tr>
                                                                      <td> <dxe:ASPxCheckBox ID="chkResPersaddr" runat="server" ClientInstanceName="CchkResPersaddr" TabIndex="8"></dxe:ASPxCheckBox></td>
                                                                      <td>
                                                                          <dxe:ASPxLabel ID="lblResPersaddr"  runat="server" Text="Change of Address of Responsible person since last Return">
                                                              </dxe:ASPxLabel>
                                                                          <span id="spResPersaddr" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                                      </td>
                                                                  </tr>
                                                            </table>
                                                  </div>
                                            </div>
                                            </div> 
                                          
                                        <div class="clear"></div>        
                                         <%--/3rd block start/--%>
                                        <div class="boxLayout clearfix">
                                                  <div class="hdBoxLayout">OTHER INFORMATION</div>
                                                  <div class="col-md-3">
                                                <label>State Name</label>
                                                <div class="relative">
                                                    <dxe:ASPxComboBox ID="drdTDSState" ClientInstanceName="cdrdTDSState" Enabled="true"
                                                          runat="server" Width="100%" MaxLength="20" DataSourceID="drTDSState" ValueField="TDSstateCode" TextField="TDSstatename">
                                                           </dxe:ASPxComboBox>

                                                    <asp:SqlDataSource runat="server" ID="drTDSState" SelectCommand="select TDSState TDSstateCode,state TDSstatename  from tbl_master_state"></asp:SqlDataSource>
                                                </div>
                                              </div>

                                                    <div class="col-md-3">
                                                <label>PAO Code</label>
                                                <div>
                                                      <asp:TextBox ID="drdpao" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                              </div>

                                                    <div class="col-md-3">
                                                <label>DDO Code</label>
                                                <div class="relative">
                                                     <asp:TextBox ID="drdDDO" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                   
                                                </div>
                                              </div>
                                        <div class="col-md-3">
                                                <label>Ministry Name</label>
                                                <div class="relative">
                                                   <dxe:ASPxComboBox ID="drdMinstryName" ClientInstanceName="cdrdMinstryName" Enabled="true"
                                                          runat="server" Width="100%" MaxLength="20" DataSourceID="drdministry" ValueField="Minstry_Code" TextField="Minstry_Name">
                                                           </dxe:ASPxComboBox>
                                                    <asp:SqlDataSource runat="server" ID="drdministry" SelectCommand="select Minstry_Name,Minstry_Code  from master_Ministry"></asp:SqlDataSource>
                                                </div>
                                              </div>
                                        <div class="clear"></div>

                                                    <div class="col-md-3">
                                                <label>Ministry Name Other</label>
                                                <div class="relative">
                                                   <asp:TextBox ID="txtOtherMinstryName" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                              </div>


                                                    <div class="col-md-3">
                                                <label>PAN of Responsible Person<span style="color:red;"> *</span> </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtRePanPers" runat="server" Width="100%" MaxLength="10"></asp:TextBox>
                                                    <span id="spRePanPers" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                </div>
                                              </div>

                                                    <div class="col-md-3">
                                                <label>PAO Registration No</label>
                                                <div>
                                                    <asp:TextBox ID="txtPaoNo" runat="server" Width="100%" MaxLength="7"></asp:TextBox>
                                                </div>
                                              </div>

                                                    <div class="col-md-3">
                                                <label>DDO Registration No</label>
                                                <div>
                                                    <asp:TextBox ID="txtDdoNo" runat="server" Width="100%" MaxLength="10"></asp:TextBox>
                                                </div>
                                              </div>
                                        <div class="clear"></div>
                                                    <div class="col-md-3">
                                                <label>Employer / Deductor's STD code (Alternate)</label>
                                                <div>
                                                    <asp:TextBox ID="txtEmpaltSTD" runat="server" Width="100%" MaxLength="5"></asp:TextBox>
                                                </div>
                                              </div>

                                                    <div class="col-md-3">
                                                <label>Employer / Deductor 's Tel-Phone No. (Alternate)</label>
                                                <div>
                                                    <asp:TextBox ID="txtEmpAltTel" runat="server" Width="100%" MaxLength="10"></asp:TextBox>
                                                </div>
                                              </div>


                                                  <div class="col-md-3">
                                                <label>Employer / Deductor Email ID (Alternate)</label>
                                                <div>
                                                    <asp:TextBox ID="txtEmpAltEmail" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                              </div>


                                                   <div class="col-md-3">
                                                <label>Responsible Person's STD Code (Alternate)</label>
                                                <div>
                                                    <asp:TextBox ID="txtPersAltSTD" runat="server" Width="100%" MaxLength="5"></asp:TextBox>
                                                </div>
                                              </div>
                                        <div class="clear"></div>
                                                   <div class="col-md-3">
                                                <label>Responsible Person's Tel-Phone No. (Alternate)</label>
                                                <div>
                                                    <asp:TextBox ID="txtPersAltTel" runat="server" Width="100%" MaxLength="10"></asp:TextBox>
                                                </div>
                                              </div>

                                                   <div class="col-md-3">
                                                <label>Responsible Person's Email ID (Alternate)</label>
                                                <div>
                                                    <asp:TextBox ID="txtResPersEmail" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                              </div>


                                                   <div class="col-md-3">
                                                <label>Account Office Identification Number (AIN) of PAO/ TO/ CDDO </label>
                                                <div>
                                                    <asp:TextBox ID="txtAcctAIN" runat="server" Width="100%" MaxLength="7"></asp:TextBox>
                                                </div>
                                              </div>

                                                   <div class="col-md-3">
                                                <label>Goods and Service Tax Number (GSTN)</label>
                                                <div>
                                                    <asp:TextBox ID="txtGST" runat="server" Width="100%" MaxLength="15"></asp:TextBox>
                                                </div>
                                              </div>
                                             </div>
                                        <div class="clear"></div>
                                             
                                                  
                                            <div class="clear"></div>

                                                   


                                        <div class="clear"></div>
                                        <div class="" style="margin-top: 8px;">
                                            <asp:Button ID="btndeductSave" runat="server" Text="Save"  CssClass="btn btn-primary"  OnClick="Save_deductorInfo"
                                              OnClientClick="if(!IsDeductorDataValid()){ return false;}"       Width="73px"  />
                                            
                                         </div>



                                                         
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <%-- End of Srijeeta mantis issue 0024438--%>
                        
                        </TabPages>
                        <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
	                                            
	                                            var Tab2 = page.GetTab(2);
	                                           
	                                           
	                                            
	                                            if(activeTab == Tab2)
	                                            {
	                                                disp_prompt('tab2');
	                                            }
	                                            if(activeTab == page.GetTab(3))
	                                            {
	                                                disp_prompt('tab3');
	                                            }
	                                            if(activeTab == page.GetTab(4))
	                                            {
	                                                disp_prompt('tab4');
	                                            }
                                                
	                                            }"></ClientSideEvents>
                    </dxe:ASPxPageControl>

                    <asp:SqlDataSource ID="sqlCompany" runat="server" SelectCommand="select cmp_internalId,cmp_name from tbl_master_company"></asp:SqlDataSource>

                    <asp:SqlDataSource ID="SqlExchange" runat="server" SelectCommand="select exch_internalId,(select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+ exch_segmentId as Exchange from tbl_master_companyExchange where exch_compId=@CompID">
                        <SelectParameters>
                            <asp:SessionParameter Name="CompID" SessionField="ID" Type="string" />
                        </SelectParameters>
                    </asp:SqlDataSource>

                    <asp:SqlDataSource ID="SqlParentTerminal" runat="server" SelectCommand="select distinct TradingTerminal_TerminalID from Master_TradingTerminal"></asp:SqlDataSource>

                    <asp:SqlDataSource ID="SqlVendor" runat="server" SelectCommand="select CTCLVendor_ID,CTCLVendor_Name+' ['+CTCLVendor_ProductType+']' as CTCLVendor_Name from Master_CTCLVendor where CTCLVendor_ExchangeSegment=@CompID1">
                        <SelectParameters>
                            <asp:SessionParameter Name="CompID1" SessionField="ID1" Type="string" />
                        </SelectParameters>
                    </asp:SqlDataSource>

                    <asp:SqlDataSource ID="TrdTerminal" runat="server" SelectCommand="select td.TradingTerminal_ID,e.exh_shortName+'-'+ce.exch_segmentId as Exchange,td.TradingTerminal_TerminalID,td.TradingTerminal_ParentTerminalID,td.TradingTerminal_ProTradeID,td.TradingTerminal_brokerid ,(select isnull(cnt_firstName,'') +' '+isnull(cnt_middleName,'')+' '+isnull(cnt_lastName,'')+ ' [' + isnull(ltrim(rtrim(cnt_UCC)),'') + '] ' from tbl_master_contact where cnt_internalId=td.TradingTerminal_BrokerID) as brokid,(Select  ISNULL(ltrim(rtrim(cnt_firstName)), '') + ' ' + ISNULL(ltrim(rtrim(cnt_middleName)), '')   + ' ' + ISNULL(ltrim(rtrim(cnt_lastName)), '') + ' [' + isnull(ltrim(rtrim(cnt_UCC)),'') + '] ' AS cnt_firstName from tbl_master_contact WHERE  cnt_internalid =td.TradingTerminal_ProTradeID) as ProTradeID,(Select  ISNULL(ltrim(rtrim(cnt_firstName)), '') + ' ' + ISNULL(ltrim(rtrim(cnt_middleName)), '')   + ' ' + ISNULL(ltrim(rtrim(cnt_lastName)), '') + ' [' + isnull(ltrim(rtrim(cnt_UCC)),'') + '] ' AS cnt_firstName from tbl_master_contact WHERE  cnt_internalid =td.TradingTerminal_CliTradeID) as CliTradeID,(Select  ISNULL(ltrim(rtrim(cnt_firstName)), '') + ' ' + ISNULL(ltrim(rtrim(cnt_middleName)), '')   + ' ' + ISNULL(ltrim(rtrim(cnt_lastName)), '') + ' [' + isnull(ltrim(rtrim(cnt_UCC)),'') + '] ' AS cnt_firstName from tbl_master_contact WHERE  cnt_internalid =td.TradingTerminal_AllTradeID) as AllTradeID from  Master_TradingTerminal td, tbl_master_exchange e, tbl_master_companyExchange ce where td.TradingTerminal_CompanyID=ce.exch_compId  and td.TradingTerminal_ExchangeSegmentID=ce.exch_InternalId and  e.exh_cntId=ce.exch_exchId and td.TradingTerminal_BranchID=@BranchID order by TradingTerminal_ID desc"
                      DeleteCommand="delete from Master_TradingTerminal where TradingTerminal_ID=@TradingTerminal_ID">
                       <SelectParameters>
                       <%--rev srijeeta ---%>
                        <%--<asp:SessionParameter Name="BranchID" SessionField="KeyVal_InternalID" Type="string" />--%>
                       <asp:SessionParameter Name="BranchID" SessionField="LastCompany" Type="string" />
                       <%-- end of rev srijeeta---%>
                        </SelectParameters>
                        <DeleteParameters>
                            <asp:Parameter Name="TradingTerminal_ID" Type="Int32" />
                        </DeleteParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>
        </table>
    </div>
    </div>
</asp:Content>

 