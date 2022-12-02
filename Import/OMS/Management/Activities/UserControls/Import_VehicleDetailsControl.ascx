<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Import_VehicleDetailsControl.ascx.cs" Inherits="ERP.OMS.Management.Activities.UserControls.Import_VehicleDetailsControl" %>
<style>
    .modal-dialog {
        width: 900px !important;
    }

    .chosen-container.chosen-container-multi,
    .chosen-container.chosen-container-single {
        width: 100% !important;
    }

    .chosen-choices {
        width: 100% !important;
    }

    #lstAssignTo {
        width: 200px;
    }
</style>
<script src="/assests/pluggins/choosen/choosen.min.js"></script>
<script>
    var flag = true;
    var ucVehflag = true;
    var forceCall = false;

    var url = getUrlVars();
    var firstCall = false;
    var tagDocFlag = false;
    $(document).ready(function () {

        if (url.key != 'ADD') {
            if (cddl_Transporter.GetValue() != '' && cddl_Transporter.GetValue() != null) {
                firstCall = true;
                $("#vehicleDeptCtrl").show();
                SwitchVechileControl(cddl_TransporterType.GetValue());
                registeredCheckChangeEvent();
                SaveVehicleControlData();
            }
        }
        ListBind();
        ListAssignTo();

    });

    function callTransporterControl(docID, docType) {
        forceCall = true;
        firstCall = true;
        tagDocFlag = true;
        ccallBackuserControlPanelMain.PerformCallback('E~' + docID + '~' + docType);
    }
    //Added By :Subhabrata
    function callTransporterControlFromPending(docID, docType, Flag) {
        forceCall = true;
        $("#DivFreight").show();
        $("#<%=hddnFlagFromPendingDlv.ClientID%>").val(Flag);
        ccallBackuserControlPanelMain.PerformCallback('E~' + docID + '~' + docType);
    }

    function callTransporterControlFromUndelivered(docID, docType, Flag) {
        debugger;
        forceCall = true;
        $("#DivFreight").show();
        $("#<%=hddnFlagFromPendingDlv.ClientID%>").val("UnDeliveredList");
        ccallBackuserControlPanelMain.PerformCallback('E~' + docID + '~' + docType);
    }
    //End

    function ccallBackuserControlPanel_EndCallback() {
        debugger;
        var FlagVal = $("#<%=hddnFlagFromPendingDlv.ClientID%>").val();
        if (FlagVal == 'PendingDelivery') {
            $("#btntransporter").hide();
            $("#vehicleTransporter").hide();
            $("#PendingDlvTransporter").show();
            $("#vehicleDeptCtrl").show();
            $("#td_FinalTransporter").show();
            SwitchVechileControl(cddl_TransporterType.GetValue());
            registeredCheckChangeEvent();
            ListBind();
            ListAssignTo();
            $("#DivFreight").show();
            $("#Div_VehicleOutDate").show();
            //SaveTransporterWithChallanDocId();

        }
        else if (FlagVal == 'UnDeliveredList')
        {

            $("#callBackuserControlPanel").hide();
            $("#DivFreight").hide();
            $("#FreghtField").hide();
            $("#Div_VehicleOutDate").show();
            $("#cAddress").hide();
            $("#cCNBilityLRNo").hide();
            $("#cTotalTaxes").hide();
            $("#cTolltax").hide();
            $("#cWeightChanrges").hide();
            $("#cUnloading").hide();
            $("#cLoading").hide();
            $("#cPoint").hide();
            $("#FreghtField").hide();
            $("#divGSTIN").hide();

            $("#cParking").hide();
            $("#cPhoneNo").hide();
            $("#cArea").hide();
            $("#cPin").hide();
            $("#cDistrict").hide();
            $("#cState").hide();
            $("#cCountry").hide();
            

            $("#btntransporter").hide();
            $("#vehicleTransporter").hide();
            $("#PendingDlvTransporter").show();
            $("#vehicleDeptCtrl").show();
            
            
            $("#td_FinalTransporter").show();
            SwitchVechileControl(cddl_TransporterType.GetValue());
            registeredCheckChangeEvent();
            ListBind();
            ListAssignTo();
            
        }
        else {
            $("#DivFreight").hide();
            if (forceCall == true) {
                if (cddl_Transporter.GetValue() != '' && cddl_Transporter.GetValue() != null) {
                    $("#vehicleDeptCtrl").show();
                    SwitchVechileControl(cddl_TransporterType.GetValue());
                    registeredCheckChangeEvent();
                    ListBind();
                    ListAssignTo();
                    SaveVehicleControlData();
                }
                forceCall = false;
            }
        }


    }

    function registeredCheckChangeEvent() {

        if ($("#radioregistercheck").find(":checked").val() == '1') {
            $("#divGSTIN").show();
        }
        else {
            $("#divGSTIN").hide();
            ctxtGSTIN1.SetText('');
            ctxtGSTIN2.SetText('');
            ctxtGSTIN3.SetText('');
        }
    }

    function getUrlVars() {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
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

    function ListAssignTo() {

        $('#lstAssignTo').chosen();
        $('#lstAssignTo').fadeIn();
    }

    function VehicleBind(listVehicle) {

        var lAssignTo = $('select[id$=lstAssignTo]');
        lAssignTo.empty();
        $('#lstAssignTo').trigger("chosen:updated");
        var listItems = [];
        $(listVehicle).each(function (index, item) {

            listItems.push('<option value="' +
               item + '" select>' + item
               + '</option>');
        });

        $(lAssignTo).append(listItems.join(''));

        ListAssignTo();
        var lstTaxRates_Ctrl = document.getElementById("lstAssignTo");

        if ($('#VehicleNo_hidden').val() != '') {
            var listVehicle_DB = $('#VehicleNo_hidden').val().split(',');
            $(listVehicle_DB).each(function (index, item) {
                for (var i = 0; i < lstTaxRates_Ctrl.options.length; i++) {
                    if (lstTaxRates_Ctrl.options[i].value == item) {
                        lstTaxRates_Ctrl.options[i].selected = true;
                        break;
                    }
                }
            });
        }
        else {
            $(listVehicle).each(function (index, item) {
                for (var i = 0; i < lstTaxRates_Ctrl.options.length; i++) {
                    if (lstTaxRates_Ctrl.options[i].value == item) {
                        lstTaxRates_Ctrl.options[i].selected = true;
                        break;
                    }
                }
            });
        }
        

        $('#lstAssignTo').trigger("chosen:updated");
    }

    function modalShowHide(param) {

        switch (param) {
            case 0:
                $('#exampleModal').modal('toggle');
                //$('#exampleModal').modal({
                //show: 'false'
                //});
                break;
            case 1:
                $('#exampleModal').modal({
                    show: 'true'
                });
                break;
        }

    }

    function modalShowHideForPending(param) {

        switch (param) {
            case 0:
                //$('#exampleModal').modal('toggle');
                $('#exampleModal').modal({
                    show: 'false'
                });
                break;
            case 1:
                $('#exampleModal').modal({
                    show: 'true'
                });
                break;
        }

    }

    function BindAssign() {

        var lAssignTo = $('select[id$=lstAssignTo]');
        lAssignTo.empty();

        var lAddEdit = document.getElementById("hdnstorequrystring").value;
        $.ajax({
            type: "POST",
            data: JSON.stringify({ id: lAddEdit }),
            url: 'FinancerAddEdit.aspx/GetAllUserListBeforeSelect',
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

                    $(lAssignTo).append(listItems.join(''));
                    //   $(lSupervisor).append(listItems.join(''));


                    //   ListSupervisor();
                    ListAssignTo();
                    setWithFromAssign();


                    $('#lstAssignTo').trigger("chosen:updated");
                }
                else {
                    //$('#lstSupervisor').trigger("chosen:updated");
                    //$('#lstSupervisor').prop('disabled', true).trigger("chosen:updated");
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });


    }

    function PopulateVehicleDetails(e) {
        $('#VehicleNo_hidden').val('');
        cddl_TransporterType.PerformCallback(cddl_Transporter.GetValue());
        registeredCheckChangeEvent();
    }

    function cmbTransporterType_EndCallBack() {
        debugger;
        if (cddl_TransporterType.cpBind != '') {
            var vehicleDetails = cddl_TransporterType.cpBind;

            cddl_TransporterType.SetValue(vehicleDetails[0]);

            $('#radioregistercheck').find("input[value='" + vehicleDetails[1] + "']").prop("checked", true)
            if (vehicleDetails[2] != '') {
                $('#divGSTIN').show();
                ctxtGSTIN1.SetText(vehicleDetails[2].substring(0, 2));
                ctxtGSTIN2.SetText(vehicleDetails[2].substring(2, 12));
                ctxtGSTIN3.SetText(vehicleDetails[2].substring(12, 15));
            }

            SwitchVechileControl(vehicleDetails[0]);
            var vehicleNo = (vehicleDetails[3]).split(',');
            VehicleBind(vehicleNo);

            var address = (vehicleDetails[4]).split('|');
            if (address.length > 0)
                CorrespondenceBind(address);
            ctxtPhone.SetText(vehicleDetails[5]);
            registeredCheckChangeEvent();
            $('#vehicleDeptCtrl').show();
            ccallBackuserControlPanel.PerformCallback('N~' + '' + '~' + '' + '~' + vehicleDetails[4]);
        }
    }

    function CorrespondenceBind(address) {
        ctxtAddress.SetText(address[0]);
    }

    function SwitchVechileControl(transType) {

        switch (transType) {
            case "54":
                $("#generalVehicle").hide();
                $("#localVehicle").show();
                if ($('#VehicleNo_hidden').val() != '') {
                    $('select[id$=lstAssignTo]').empty();
                    $('#lstAssignTo').trigger("chosen:updated");
                    VehicleBind($('#MasterVehicleNo_hidden').val().split(','));
                }
                //if ($('#VehicleNo_hidden').val() != '') {
                //    document.getElementById("executiveTable").innerHTML = '';
                //    var veh = $('#VehicleNo_hidden').val().split(',');
                //    $(veh).each(function (index, item) {
                //        AddNewexecutive(item);
                //    });
                //}
                break;
            case "55":
                $("#localVehicle").hide();
                $("#generalVehicle").show();
                //if ($('#VehicleNo_hidden').val() != '') {
                //    $('select[id$=lstAssignTo]').empty();
                //    $('#lstAssignTo').trigger("chosen:updated");
                //    VehicleBind($('#MasterVehicleNo_hidden').val().split(','));
                //}
                if ($('#VehicleNo_hidden').val() != '') {
                    document.getElementById("executiveTable").innerHTML = '';
                    var veh = $('#VehicleNo_hidden').val().split(',');
                    $(veh).each(function (index, item) {
                        AddNewexecutive(item);
                    });
                }
                break;
        }
    }

    function SaveVehicleControlData() {


        var gstin = '';
        if (cddl_TransporterType.GetValue() == "54") {
            SetLocalVehicleNo();
        }
        else if (cddl_TransporterType.GetValue() == "55") {
            SetGeneralVehicleNo();
        }
        if ($("#radioregistercheck").find(":checked").val() == "1") {
            gstin = ctxtGSTIN1.GetValue() + ctxtGSTIN2.GetValue() + ctxtGSTIN3.GetValue();
        }

        //var data = cddl_Transporter.GetValue() + "|"; //Transporter ID
        //data += cddl_TransporterType.GetValue() + "|"; // Transporter Type
        //data += $("#radioregistercheck").find(":checked").val() + "|"; //Registered
        //data += gstin + "|"; //GSTIN
        //data += $('#VehicleNo_hidden').val() + "|"; // VehicleNos
        //data += ctxtAddress.GetText() + "|"; // Address
        //data += (cddl_Country.GetValue() == ',') ? '' : cddl_Country.GetValue() + "|"; // Country
        //data += (cddl_State.GetValue() == ',') ? '' : cddl_State.GetValue() + "|";
        //data += (cddl_City.GetValue() == ',') ? '' : cddl_City.GetValue() + "|";
        //data += (cddl_Pin.GetValue() == ',') ? '' : cddl_Pin.GetValue() + "|";
        //data += (cddl_Area.GetValue() == ',') ? '' : cddl_Area.GetValue() + "|";
        //data += ctxtPhone.GetText(); // Phone

        //var VehicleOutDate = '';
        //if (cdtVehicleOutDate.GetDate() != null) {
        //    var jsDate = cdtVehicleOutDate.GetDate();
        //    var year = jsDate.getFullYear(); // where getFullYear returns the year (four digits)
        //    var month = jsDate.getMonth(); // where getMonth returns the month (from 0-11)
        //    var day = jsDate.getDate();   // where getDate returns the day of the month (from 1-31)
        //    var VehicleOutDate = new Date(year, month, day).toLocaleDateString('en-GB');
        //}

        var data = cddl_Transporter.GetValue() + "|"; //Transporter ID
        data += cddl_TransporterType.GetValue() + "|"; // Transporter Type
        data += $("#radioregistercheck").find(":checked").val() + "|"; //Registered
        data += gstin + "|"; //GSTIN
        data += $('#VehicleNo_hidden').val() + "|"; // VehicleNos
        data += ctxtAddress.GetText() + "|"; // Address
        data += (cddl_Country.GetValue() == ',') ? '' : cddl_Country.GetValue() + "|"; // Country
        data += (cddl_State.GetValue() == ',') ? '' : cddl_State.GetValue() + "|"; //state
        data += (cddl_City.GetValue() == ',') ? '' : cddl_City.GetValue() + "|"; //city
        data += (cddl_Pin.GetValue() == ',') ? '' : cddl_Pin.GetValue() + "|"; //pin
        data += (cddl_Area.GetValue() == ',') ? '' : cddl_Area.GetValue() + "|"; //area
        data += ctxtPhone.GetText() + "|"; // Phone
        data += ctxtFreight.GetText() + "|"; // Freight
        data += ctxtPoint.GetText() + "|"; // Point
        data += ctxtLoading.GetText() + "|"; // Loading
        data += ctxtUnloading.GetText() + "|"; // Unloading
        data += ctxtParking.GetText() + "|"; // Parking
        data += ctxtWeighment.GetText() + "|";// Weighment
        data += ctxtLrno.GetText() + "|"; // CN / Bilty / LR No. 
        data += cdtVehicleOutDate.GetText() + "|";  // Vehicle Out Date & Time
        data += ctxtTollTax.GetText() + "|"; //Toll Tax
        data += cvcCmbTrip.GetValue() + "|"; //Trip
        data += cvcCmbFreightArea.GetValue() + "|"; //Freight Area
        data += ctxtTotalCharges.GetText(); //TotalCharges

        var validFlag = Validation();

        if (tagDocFlag == true && $("#exampleModal").is(":hidden")) {
            tagDocFlag = false;
            $("#hfControlSaveData").val(data);
        }
        else {
            if (url.key != 'ADD' && firstCall == true) {
                firstCall = false;
                $("#hfControlSaveData").val(data);

            }
            else {
                if (validFlag == true && ucVehflag == true) {
                    // alert(data);
                    $("#hfControlSaveData").val(data);

                    modalShowHide(0);
                }
                else {

                    modalShowHide(1);
                }
            }
        }
    }


    function SaveTransporterlData() {

        debugger;
        var gstin = '';
        if (cddl_TransporterType.GetValue() == "54") {
            SetLocalVehicleNo();
        }
        else if (cddl_TransporterType.GetValue() == "55") {
            SetGeneralVehicleNo();
        }
        if ($("#radioregistercheck").find(":checked").val() == "1") {
            gstin = ctxtGSTIN1.GetValue() + ctxtGSTIN2.GetValue() + ctxtGSTIN3.GetValue();
        }



        var data = cddl_Transporter.GetValue() + "|"; //Transporter ID
        data += cddl_TransporterType.GetValue() + "|"; // Transporter Type
        data += $("#radioregistercheck").find(":checked").val() + "|"; //Registered
        data += gstin + "|"; //GSTIN
        data += $('#VehicleNo_hidden').val() + "|"; // VehicleNos
        data += ctxtAddress.GetText() + "|"; // Address
        data += (cddl_Country.GetValue() == ',') ? '' : cddl_Country.GetValue() + "|"; // Country
        data += (cddl_State.GetValue() == ',') ? '' : cddl_State.GetValue() + "|"; //state
        data += (cddl_City.GetValue() == ',') ? '' : cddl_City.GetValue() + "|"; //city
        data += (cddl_Pin.GetValue() == ',') ? '' : cddl_Pin.GetValue() + "|"; //pin
        data += (cddl_Area.GetValue() == ',') ? '' : cddl_Area.GetValue() + "|"; //area
        data += ctxtPhone.GetText() + "|"; // Phone
        data += ctxtFreight.GetText() + "|"; // Freight
        data += ctxtPoint.GetText() + "|"; // Point
        data += ctxtLoading.GetText() + "|"; // Loading
        data += ctxtUnloading.GetText() + "|"; // Unloading
        data += ctxtParking.GetText() + "|"; // Parking
        data += ctxtWeighment.GetText() + "|";// Weighment
        data += ctxtLrno.GetText() + "|"; // CN / Bilty / LR No. 
        data += cdtVehicleOutDate.GetText() + "|";  // Vehicle Out Date & Time
        data += ctxtTollTax.GetText() + "|"; //Toll Tax
        data += cvcCmbTrip.GetValue() + "|"; //Trip
        data += cvcCmbFreightArea.GetValue() + "|"; //Freight Area
        data += ctxtTotalCharges.GetText() + "|"; //TotalCharges
        data += cddl_FinalTransporter.GetValue(); //FinalTransporter

        var validFlag = Validation();
        if (document.getElementById('VehicleNo_hidden').value.split(',').length > 1) {
            jAlert('Multiple Vehicle No is not allowed.', "Alert Message!!!", function () {

            });
            validFlag = false;

            modalShowHideForPending(1);
        }
        else if (cdtVehicleOutDate.GetValue() == null) {
            

            jAlert('Vehicle Out Date is required.', "Alert Message!!!", function () {
                cdtVehicleOutDate.Focus();
            });
            validFlag = false;

            modalShowHide(1);
        }
        //else if (cdtVehicleOutDate.GetValue() != null)
        //{
        //    debugger;
        //    var FinalDate = cdtVehicleOutDate.GetDate();
        //    var Minute = FinalDate.getMinutes();
        //    var hours = FinalDate.getHours();

        //    if (hours=== 0)
        //    {
        //        jAlert('Vehicle Out Time is required.', "Alert Message!!!", function () {
        //            cdtVehicleOutDate.Focus();
        //        });

        //        validFlag = false;

        //        modalShowHide(1);
        //    }
        //}

        if (url.key != 'ADD' && firstCall == true) {
            firstCall = false;
            $("#hfControlSaveData").val(data);


        }
        else {
            if (validFlag == true && ucVehflag == true) {
                // alert(data);
                $("#hfControlSaveData").val(data);

                //modalShowHide(0);
                modalShowHideForPending(0);
                SaveTransporterWithChallanDocId();
            }
            else {

                //modalShowHide(1);
                modalShowHideForPending(1);
            }
        }

    }


    function AddNewexecutive(value) {

        value = (value == 0) ? '' : value;
        var table = document.getElementById("executiveTable");
        $("#executiveTable").addClass('error');
        var row = table.insertRow(0);
        var cell1 = row.insertCell(0);
        var cell2 = row.insertCell(1);

        cell1.innerHTML = "<input type='text' class='focusMe' maxlength = '18' value='" + value + "' />";
        cell2.innerHTML = "<button type='button' value='' onclick='AddNewexecutive(0)' class='btn btn-primary btn-xs' style='margin-top: -5px;'><i class='fa fa-plus-circle'></i></button> <button type='button' value='' class='btn btn-danger btn-xs' style='margin-top: -5px;' onclick='removeExecutive(this.parentNode.parentNode)'><i class='fa fa-times-circle'></i></button>";
        $('.focusMe').eq($('.focusMe').length - 1).focus()

        $("#executiveTable").find('.focusMe').removeClass("focusMe");
    }

    function removeExecutive(obj) {
        var rowIndex = obj.rowIndex;
        var table = document.getElementById("executiveTable");
        if (table.rows.length > 1) {
            table.deleteRow(rowIndex);
        } else {
            jAlert('Cannot delete all Vechile.');
        }
    }

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

    function cmbTransporterType_change() {

        SwitchVechileControl(cddl_TransporterType.GetValue());
    }

    function ucTransporterTrimString(str) {
        return str.replace(/^\s\s*/, '').replace(/\s\s*$/, '');
    }

    function SetLocalVehicleNo() {
        if ($("#lstAssignTo").val() != '' && $("#lstAssignTo").val() != undefined) {
            $('#VehicleNo_hidden').val($("#lstAssignTo").val().join());
        }


        localflag = true;
        ucVehflag = true;

        //var localflag = true;
        //var table = document.getElementById("executiveTable");

        //document.getElementById('VehicleNo_hidden').value = '';
        //var data = '';
        //for (var i = 0, row; row = table.rows[i]; i++) {
        //    for (var j = 0, col; col = row.cells[j]; j++) {
        //        if (col.children[0].type != 'button') {
        //            if (data == '') {
        //                
        //                if (ucTransporterTrimString(col.children[0].value) == '') {

        //                    //jAlert("Vehicle number required", "Alert Message!!!", function () {

        //                    //    col.children[0].focus();
        //                    //    return false;
        //                    //});
        //                    modalShowHide(1);

        //                    //localflag = false;
        //                    //ucVehflag = ucVehflag && localflag;
        //                    //return false;

        //                    localflag = true;
        //                    ucVehflag = true;
        //                    return true;
        //                }
        //                else {
        //                    data = col.children[0].value;
        //                    // ucVehflag = (localflag == false) ? false : true;

        //                    ucVehflag = true;

        //                    //// flag = (localflag == false) ? false : true;

        //                    ////if ($('#hfLocalVehFlag').val() != 0) {
        //                    ////    $("#executiveTable").removeClass('error');
        //                    ////    flag = true;
        //                    ////} 
        //                }
        //            }
        //            else {
        //                if (col.children[0].value == '') {
        //                    jAlert("Required");
        //                    //  return false;
        //                    //flag = false;
        //                    ucVehflag = false;
        //                }
        //                else {
        //                    data = data + '~' + col.children[0].value;
        //                }
        //            }
        //        }
        //    }

        //    if (document.getElementById('VehicleNo_hidden').value == '') {
        //        document.getElementById('VehicleNo_hidden').value = data;
        //        data = '';
        //    }
        //    else {
        //        document.getElementById('VehicleNo_hidden').value = document.getElementById('VehicleNo_hidden').value + ',' + data;
        //        data = '';
        //    }
        //}

        return ucVehflag;
    }

    function SetGeneralVehicleNo() {

        //$('#VehicleNo_hidden').val($("#lstAssignTo").val().join());
        var localflag = true;
        var table = document.getElementById("executiveTable");

        document.getElementById('VehicleNo_hidden').value = '';
        var data = '';
        for (var i = 0, row; row = table.rows[i]; i++) {
            for (var j = 0, col; col = row.cells[j]; j++) {
                if (col.children[0].type != 'button') {
                    if (data == '') {

                        if (ucTransporterTrimString(col.children[0].value) == '') {

                            //jAlert("Vehicle number required", "Alert Message!!!", function () {

                            //    col.children[0].focus();
                            //    return false;
                            //});

                            // modalShowHide(1);

                            //localflag = false;
                            //ucVehflag = ucVehflag && localflag;
                            //return false;

                            localflag = true;
                            ucVehflag = true;
                            return true;
                        }
                        else {
                            data = col.children[0].value;
                            // ucVehflag = (localflag == false) ? false : true;

                            ucVehflag = true;

                            //// flag = (localflag == false) ? false : true;

                            ////if ($('#hfLocalVehFlag').val() != 0) {
                            ////    $("#executiveTable").removeClass('error');
                            ////    flag = true;
                            ////} 
                        }
                    }
                    else {
                        if (col.children[0].value == '') {
                            //jAlert("Required");
                            ////  return false;
                            ////flag = false;
                            //ucVehflag = false;
                        }
                        else {
                            data = data + '~' + col.children[0].value;
                        }
                    }
                }
            }

            if (document.getElementById('VehicleNo_hidden').value == '') {
                document.getElementById('VehicleNo_hidden').value = data;
                data = '';
            }
            else {
                document.getElementById('VehicleNo_hidden').value = document.getElementById('VehicleNo_hidden').value + ',' + data;
                data = '';
            }
        }
    }

    function UcOnCountryChanged(vcCmbCountry) {
        cddl_State.PerformCallback(cddl_Country.GetValue().toString());
    }

    function UcOnStateChanged(vcCmbState) {

        cddl_City.PerformCallback((cddl_State.GetValue() != null) ? cddl_State.GetValue().toString() : "");
    }

    function UcOnCityChanged(vcCmbCity) {

        cddl_Pin.PerformCallback((cddl_City.GetValue() != null) ? cddl_City.GetValue().toString() : "");
        cddl_Area.PerformCallback((cddl_City.GetValue() != null) ? cddl_City.GetValue().toString() : "");
    }

    function CloseModal() {
        $('#exampleModal').modal({
            show: 'false'
        });
    }

    function Validation() {
        flag = true;
        if ($("#radioregistercheck").find(":checked").val() == '1') {
            if (ctxtGSTIN1.GetValue() == '' || ctxtGSTIN1.GetValue() == null) {
                flag = false;
                SetMandatory("ctxtGSTIN1");

                jAlert('GSTIN number is Mandatory', "Alert Message!!!", function () {
                    ctxtGSTIN1.Focus();
                });

                modalShowHide(1);
            }
            else if (ctxtGSTIN2.GetValue() == '' || ctxtGSTIN2.GetValue() == null) {
                flag = false;
                SetMandatory("ctxtGSTIN2");
                jAlert('GSTIN number is Mandatory', "Alert Message!!!", function () {
                    ctxtGSTIN2.Focus();
                });

                modalShowHide(1);
            }
            else if (ctxtGSTIN3.GetValue() == '' || ctxtGSTIN3.GetValue() == null) {
                flag = false;
                SetMandatory("ctxtGSTIN3");
                jAlert('GSTIN number is Mandatory', "Alert Message!!!", function () {
                    ctxtGSTIN3.Focus();
                });

                modalShowHide(1);
            }
            else if (!GSTINCheck()) {
                jAlert('GSTIN is not a valid number', "Alert Message!!!", function () {
                    ctxtGSTIN2.Focus();
                });
                flag = false;

                modalShowHide(1);
            }
        }


        return flag;
    }

    function SetMandatory(refAttr) {
        $('#Mandatorytxt_' + refAttr).show();
    }

    function GSTINCheck() {
        var returnvalue = true;
        var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
        var code = /([C,P,H,F,A,T,B,L,J,G])/;
        var gsTIN2 = ctxtGSTIN2.GetValue();
        if (gsTIN2 != "" || gsTIN2 != null) {
            var code_chk = gsTIN2.substring(3, 4);
            if (gsTIN2.search(panPat) == -1) {
                returnvalue = false;
            }
            if (code.test(code_chk) == false) {
                returnvalue = false;
            }
        }
        else {
            returnvalue = false;
        }

        return returnvalue;
    }

    function clearTransporter() {
        cddl_Transporter.SetText('--Select--');
        $('#hfControlSaveData').val('');
        $('#vehicleDeptCtrl').hide();
    }

    function CalculateTotalCharges() {
        var totalCharges = 0;

        if (ctxtFreight.GetText() != null && ctxtFreight.GetText() != '') {
            totalCharges += parseFloat(ctxtFreight.GetText());
        }
        if (ctxtPoint.GetText() != null && ctxtPoint.GetText() != '') {
            totalCharges += parseFloat(ctxtPoint.GetText());
        }
        if (ctxtLoading.GetText() != null && ctxtLoading.GetText() != '') {
            totalCharges += parseFloat(ctxtLoading.GetText());
        }
        if (ctxtUnloading.GetText() != null && ctxtUnloading.GetText() != '') {
            totalCharges += parseFloat(ctxtUnloading.GetText());
        }
        if (ctxtParking.GetText() != null && ctxtParking.GetText() != '') {
            totalCharges += parseFloat(ctxtParking.GetText());
        }
        if (ctxtWeighment.GetText() != null && ctxtWeighment.GetText() != '') {
            totalCharges += parseFloat(ctxtWeighment.GetText());
        }
        if (ctxtTollTax.GetText() != null && ctxtTollTax.GetText() != '') {
            totalCharges += parseFloat(ctxtTollTax.GetText());
        }

        ctxtTotalCharges.SetValue(totalCharges.toString());
    }
</script>
<style>
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

    #executiveTable > tbody > tr > td:first-child {
        padding-right: 15px;
    }

    .btn {
        margin-bottom: 0 !important;
        margin-top: 8px;
    }
</style>

<button type="button" class="btn btn-primary" data-toggle="modal" data-target="#exampleModal" data-whatever="@mdo" id="btntransporter">Transporter&#818;</button>
<!-- Modal -->
<div class="modal fade" id="exampleModal" role="dialog" aria-labelledby="exampleModalLabel" data-backdrop="static">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" id="exampleModalLabel">Transporter Details</h4>
            </div>
            <div class="modal-body">
                <form>
                    <dxe:ASPxCallbackPanel runat="server" ID="callBackuserControlPanelMain" ClientInstanceName="ccallBackuserControlPanelMain" OnCallback="ComponentPanelMain_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label for="recipient-name" class="control-label">Transporter Name</label>
                                        <dxe:ASPxComboBox ID="cmbTransporter" runat="server" ClientInstanceName="cddl_Transporter" Width="100%">
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateVehicleDetails(e)}" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>
                                <div class="col-md-3" id="td_FinalTransporter" style="display:none;" >
                                    <div class="form-group" id="td_FinalTransporter1">
                                        <label for="recipient-name" class="control-label">Final Transporter</label>
                                        <dxe:ASPxComboBox ID="cmbFinalTransporter" runat="server" ClientInstanceName="cddl_FinalTransporter" Width="100%">
                                            
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>
                                <div class="clear"></div>
                                <%--<div class="clear"></div>--%>
                                <div id="vehicleDeptCtrl" style="display: none;" class="row">
                                    <div class="col-md-12">
                                        <div class="col-md-4" id="cTransporterType">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">Transporter Type</label>
                                                <dxe:ASPxComboBox ID="cmbTransporterType" runat="server" ClientInstanceName="cddl_TransporterType" OnCallback="cmbTransporterType_Callback"
                                                    Width="100%">
                                                    <ClientSideEvents EndCallback="cmbTransporterType_EndCallBack" SelectedIndexChanged="cmbTransporterType_change" />
                                                </dxe:ASPxComboBox>
                                            </div>
                                        </div>

                                        <div id="td_registered" class="labelt col-md-2">
                                            <div class="visF">
                                                <label>Registered?</label>
                                                <asp:RadioButtonList runat="server" ID="radioregistercheck" RepeatDirection="Horizontal" Width="130px">
                                                    <asp:ListItem Text="Yes" Value="1" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>

                                        <div id="divGSTIN" class="col-md-6 forCustomer">
                                            <label class="labelt">GSTINs</label>
                                            <span id="spanmandategstn" style="color: red; display: none;">*</span>
                                            <div class="relative">
                                                <ul class="nestedinput">
                                                    <li>
                                                        <dxe:ASPxTextBox ID="txtGSTIN1" ClientInstanceName="ctxtGSTIN1" MaxLength="2" runat="server" Width="33px">
                                                            <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                                        </dxe:ASPxTextBox>
                                                        <span id="Mandatorytxt_txtGSTIN1" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                                                    </li>
                                                    <li class="dash">-</li>
                                                    <li>
                                                        <dxe:ASPxTextBox ID="txtGSTIN2" ClientInstanceName="ctxtGSTIN2" MaxLength="10" runat="server" Width="90px">
                                                            <ClientSideEvents KeyUp="Gstin2TextChanged" />
                                                        </dxe:ASPxTextBox>
                                                    </li>
                                                    <li class="dash">-</li>
                                                    <li>
                                                        <dxe:ASPxTextBox ID="txtGSTIN3" ClientInstanceName="ctxtGSTIN3" MaxLength="3" runat="server" Width="90px">
                                                            <ClientSideEvents />
                                                        </dxe:ASPxTextBox>
                                                        <span id="invalidGst" class="fa fa-exclamation-circle iconRed " style="color: red; display: none; padding-left: 9px; left: 302px;" title="Invalid GSTIN"></span>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <%-- <div id="localVehicle" style="display: none">--%>
                                                <div id="generalVehicle" style="display: none">
                                                    <%-- Start Tranporter Vechile No --%>
                                                    <asp:Panel ID="pnlVehicleNo" runat="server" CssClass="">
                                                        <label style="display: block;">Vehicle No.</label>
                                                        <table id="executiveTable" style="width: 320px;" class="nbackBtn" runat="server">
                                                            <tr>
                                                                <td style="padding-right: 15px">
                                                                    <input type="text" maxlength="18" />
                                                                </td>
                                                                <td>
                                                                    <button type="button" style="margin-top: -5px" class="btn btn-primary btn-xs" onclick="AddNewexecutive('0')"><i class="fa fa-plus-circle"></i></button>
                                                                    <button type="button" style="margin-top: -5px" class="btn btn-danger btn-xs" onclick="removeExecutive(this.parentNode.parentNode)"><i class="fa fa-times-circle"></i></button>
                                                                </td>

                                                            </tr>
                                                        </table>
                                                        <asp:HiddenField ID="hfLocalVehFlag" runat="server" />
                                                        <asp:HiddenField ID="MasterVehicleNo_hidden" runat="server" />
                                                        <asp:HiddenField ID="VehicleNo_hidden" runat="server" />
                                                        <asp:HiddenField ID="hddnFlagFromPendingDlv" runat="server" />
                                                        <asp:HiddenField ID="hddnFlagUndeliveredList" runat="server" />
                                                    </asp:Panel>
                                                    <%-- END Tranporter Vechile No --%>
                                                </div>

                                                <%-- <div id="generalVehicle" style="display: none">--%>
                                                <div id="localVehicle" style="display: none">
                                                    <label for="recipient-name" class="control-label">Vehicle No</label>
                                                    <asp:ListBox ID="lstAssignTo" SelectionMode="Multiple" CssClass="hide" runat="server" Font-Size="12px" Height="90px" Width="100%" data-placeholder="Select..."></asp:ListBox>
                                                    <asp:Label ID="lblAssignTo" runat="server" Text=""></asp:Label>
                                                    <span id="MandatoryAssign" style="display: none"><span id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="pullrightClass fa fa-exclamation-circle abs iconRed" title="Mandatory"></span></span>
                                                    <asp:HiddenField ID="hdnAssign" runat="server" />
                                                    <asp:HiddenField ID="hdnAssignText" runat="server" />
                                                    <asp:HiddenField ID="hdnEditAssignTo" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clear"></div>

                                        <div class="col-md-3" id="FreghtField">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">Freight</label>
                                                <dxe:ASPxTextBox ID="txtFreight" MaxLength="80" ClientInstanceName="ctxtFreight"
                                                    runat="server" Width="100%">
                                                    <MaskSettings Mask="&lt;0..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="function(s, e) { CalculateTotalCharges(s); }"></ClientSideEvents>
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3" id="cPoint">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">Point</label>
                                                <dxe:ASPxTextBox ID="txtPoint" MaxLength="80" ClientInstanceName="ctxtPoint"
                                                    runat="server" Width="100%">
                                                    <MaskSettings Mask="&lt;0..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="function(s, e) { CalculateTotalCharges(s); }"></ClientSideEvents>
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3" id="cLoading">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">Loading</label>
                                                <dxe:ASPxTextBox ID="txtLoading" MaxLength="80" ClientInstanceName="ctxtLoading"
                                                    runat="server" Width="100%">
                                                    <MaskSettings Mask="&lt;0..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="function(s, e) { CalculateTotalCharges(s); }"></ClientSideEvents>
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3" id="cUnloading">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">Unloading</label>
                                                <dxe:ASPxTextBox ID="txtUnloading" MaxLength="80" ClientInstanceName="ctxtUnloading"
                                                    runat="server" Width="100%">
                                                    <MaskSettings Mask="&lt;0..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="function(s, e) { CalculateTotalCharges(s); }"></ClientSideEvents>
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                        <div class="col-md-3" id="cParking">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">Parking</label>
                                                <dxe:ASPxTextBox ID="txtParking" MaxLength="80" ClientInstanceName="ctxtParking"
                                                    runat="server" Width="100%">
                                                    <MaskSettings Mask="&lt;0..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="function(s, e) { CalculateTotalCharges(s); }"></ClientSideEvents>
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3" id="cWeightChanrges">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">Weighment Charges</label>
                                                <dxe:ASPxTextBox ID="txtWeighment" MaxLength="80" ClientInstanceName="ctxtWeighment"
                                                    runat="server" Width="100%">
                                                    <MaskSettings Mask="&lt;0..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="function(s, e) { CalculateTotalCharges(s); }"></ClientSideEvents>
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3" id="cTolltax">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">Toll Tax</label>
                                                <dxe:ASPxTextBox ID="txtTollTax" MaxLength="80" ClientInstanceName="ctxtTollTax"
                                                    runat="server" Width="100%">
                                                    <MaskSettings Mask="&lt;0..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="function(s, e) { CalculateTotalCharges(s); }"></ClientSideEvents>
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3" id="cTotalTaxes">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">Total Charges</label>
                                                <dxe:ASPxTextBox ID="txtTotalCharges" MaxLength="80" ClientInstanceName="ctxtTotalCharges" ReadOnly="true"
                                                    runat="server" Width="100%">
                                                    <MaskSettings Mask="&lt;0..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                        <div id="DivFreight" style="display: none;">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label for="recipient-name" class="control-label">Trip</label>
                                                    <dxe:ASPxComboBox ID="vcCmbTrip" runat="server" ClientInstanceName="cvcCmbTrip" Width="100%">
                                                        <ClearButton DisplayMode="Always"></ClearButton>
                                                        <Items>
                                                            <dxe:ListEditItem Text="Trip 1" Value="Trip 1" />
                                                            <dxe:ListEditItem Text="Trip 2" Value="Trip 2" />
                                                            <dxe:ListEditItem Text="Trip 3" Value="Trip 3" />
                                                            <dxe:ListEditItem Text="Trip 4" Value="Trip 4" />
                                                        </Items>
                                                    </dxe:ASPxComboBox>
                                                </div>
                                            </div>

                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label for="recipient-name" class="control-label">Freight Area</label>
                                                    <dxe:ASPxComboBox ID="vcCmbFreightArea" runat="server" ClientInstanceName="cvcCmbFreightArea" Width="100%">
                                                        <ClearButton DisplayMode="Always"></ClearButton>
                                                    </dxe:ASPxComboBox>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="clear"></div>
                                        <asp:Panel ID="pnl_ChallanSpeceficFields" runat="server">
                                            <div class="col-md-6" id="cCNBilityLRNo">
                                                <div class="form-group">
                                                    <label for="recipient-name" class="control-label">CN / Bilty / LR No</label>
                                                    <dxe:ASPxTextBox ID="txtLrno" MaxLength="80" ClientInstanceName="ctxtLrno"
                                                        runat="server" Width="100%">
                                                    </dxe:ASPxTextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-6" style="display: none" id="Div_VehicleOutDate">
                                                <div class="form-group">
                                                    <label for="recipient-name" class="control-label">Vehicle Out Date</label>

                                                    
                                                    <dxe:ASPxDateEdit ID="dtVehicleOutDate" ClientInstanceName="cdtVehicleOutDate" runat="server" EditFormat="Custom" Width="100%"
                                                        DisplayFormatString="dd/MM/yyyy HH:mm" EditFormatString="dd/MM/yyyy HH:mm" UseMaskBehavior="True">
                                                        <TimeSectionProperties>
                                                            <TimeEditProperties EditFormatString="HH:mm" />
                                                        </TimeSectionProperties>
                                                    </dxe:ASPxDateEdit>
                                                </div>
<%--                                                <div class="form-group">
                                                    <label for="recipient-name" class="control-label">Vehicle Time</label>

                                                    
                                                    <dxe:ASPxDateEdit ID="dtvehicleTime" ClientInstanceName="cdtVehicleTime" runat="server" EditFormat="Custom" Width="100%"
                                                        DisplayFormatString="HH:mm" EditFormatString="HH:mm" UseMaskBehavior="True">
                                                        <TimeSectionProperties>
                                                            <TimeEditProperties EditFormatString="HH:mm" />
                                                        </TimeSectionProperties>
                                                    </dxe:ASPxDateEdit>
                                                </div>--%>
                                            </div>
                                        </asp:Panel>
                                        <div class="clear"></div>
                                        <div class="col-md-12" id="cAddress">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">Address</label>
                                                <dxe:ASPxTextBox ID="txtAddress" MaxLength="80" ClientInstanceName="ctxtAddress"
                                                    runat="server" Width="100%">
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>

                                        <dxe:ASPxCallbackPanel runat="server" ID="callBackuserControlPanel" ClientInstanceName="ccallBackuserControlPanel" OnCallback="ComponentPanel_Callback">
                                            <PanelCollection>
                                                <dxe:PanelContent runat="server">

                                                    <div class="clear"></div>
                                                    <div class="col-md-4" id="cCountry">
                                                        <div class="form-group">
                                                            <label for="recipient-name" class="control-label">Country</label>
                                                            <dxe:ASPxComboBox ID="vcCmbCountry" ClientInstanceName="cddl_Country" runat="server" Width="100%">
                                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { UcOnCountryChanged(s); }"></ClientSideEvents>
                                                            </dxe:ASPxComboBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="form-group" id="cState">
                                                            <label for="recipient-name" class="control-label">State</label>
                                                            <div>
                                                                <dxe:ASPxComboBox ID="vcCmbState" ClientInstanceName="cddl_State" runat="server"
                                                                    ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0"
                                                                    OnCallback="cmbState_OnCallback">
                                                                    <ClearButton DisplayMode="Always"></ClearButton>
                                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { UcOnStateChanged(s); }"></ClientSideEvents>
                                                                </dxe:ASPxComboBox>

                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="form-group" id="cDistrict">
                                                            <label for="recipient-name" class="control-label">City/District</label>
                                                            <div>
                                                                <dxe:ASPxComboBox ID="vcCmbCity" ClientInstanceName="cddl_City" runat="server"
                                                                    OnCallback="cmbCity_OnCallback" Width="100%">
                                                                    <ClearButton DisplayMode="Always"></ClearButton>
                                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { UcOnCityChanged(s); }"></ClientSideEvents>
                                                                </dxe:ASPxComboBox>

                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="clear"></div>
                                                    <div class="col-md-4">
                                                        <div class="form-group" id="cPin">
                                                            <label for="recipient-name" class="control-label">Pin/Zip</label>
                                                            <div>
                                                                <dxe:ASPxComboBox ID="vcCmbPin" ClientInstanceName="cddl_Pin" runat="server"
                                                                    OnCallback="cmbPin_OnCallback" Width="100%">
                                                                    <ClearButton DisplayMode="Always"></ClearButton>
                                                                </dxe:ASPxComboBox>

                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="form-group" id="cArea">
                                                            <label for="recipient-name" class="control-label">Area</label>
                                                            <div>
                                                                <dxe:ASPxComboBox ID="vcCmbArea" ClientInstanceName="cddl_Area" runat="server"
                                                                    OnCallback="cmbArea_OnCallback" Width="100%">
                                                                    <ClearButton DisplayMode="Always"></ClearButton>
                                                                </dxe:ASPxComboBox>

                                                            </div>
                                                        </div>
                                                    </div>
                                                </dxe:PanelContent>
                                            </PanelCollection>
                                            <%--<clientsideevents endcallback="ccallBackuserControlPanel_EndCallback"/>--%>
                                        </dxe:ASPxCallbackPanel>

                                        <div class="col-md-4">
                                            <div class="form-group" id="cPhoneNo">
                                                <label for="recipient-name" class="control-label">Phone No</label>
                                                <dxe:ASPxTextBox ID="txtPhone" MaxLength="80" ClientInstanceName="ctxtPhone"
                                                    runat="server" Width="100%">
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                        <div class="text-center" style="text-align: center !important">
                                            <button type="button" class="btn btn-primary" id="vehicleTransporter" onclick="SaveVehicleControlData()" style="margin-bottom: 0 !important"><u>S</u>ave</button>
                                            <button type="button" class="btn btn-primary" id="PendingDlvTransporter" onclick="SaveTransporterlData()" style="margin-bottom: 0 !important; display: none;"><u>S</u>ave Transporter</button>
                                            <button type="button" class="btn btn-danger" data-dismiss="modal"><u>C</u>ancel</button>
                                        </div>
                                    </div>
                                </div>
                                <div class="clear"></div>
                                <asp:HiddenField ID="hfControlSaveData" runat="server" />
                            </dxe:PanelContent>
                        </PanelCollection>
                        <ClientSideEvents EndCallback="ccallBackuserControlPanel_EndCallback" />
                    </dxe:ASPxCallbackPanel>
                </form>
            </div>
        </div>
    </div>
</div>





