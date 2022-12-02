<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VehicleDetailsControl.ascx.cs" Inherits="OpeningEntry.UserControls.VehicleDetailsControl" %>
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
    var forceCall = false;
    $(document).ready(function () {
        debugger;
        var url = getUrlVars();
        if (url.key != 'ADD') {
            if (cddl_Transporter.GetValue() != '' && cddl_Transporter.GetValue() != null) {
                $("#vehicleDeptCtrl").show();
                SwitchVechileControl(cddl_TransporterType.GetValue());
                registeredCheckChangeEvent();
            }
        }
        ListBind();
        ListAssignTo();

    });

    function callTransporterControl(docID, docType) {
        forceCall = true;
        ccallBackuserControlPanelMain.PerformCallback('E~' + docID + '~' + docType);
    }

    function ccallBackuserControlPanel_EndCallback() {
        debugger;
        if (forceCall == true) {
            if (cddl_Transporter.GetValue() != '' && cddl_Transporter.GetValue() != null) {
                $("#vehicleDeptCtrl").show();
                SwitchVechileControl(cddl_TransporterType.GetValue());
                registeredCheckChangeEvent();
                ListBind();
                ListAssignTo();
            }
            forceCall = false;
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
        $(listVehicle).each(function (index, item) {
            for (var i = 0; i < lstTaxRates_Ctrl.options.length; i++) {
                if (lstTaxRates_Ctrl.options[i].value == item) {
                    lstTaxRates_Ctrl.options[i].selected = true;
                }
            }
        });

        $('#lstAssignTo').trigger("chosen:updated");
    }

    function modalShowHide(param) {

        switch (param) {
            case 0:
                $('#exampleModal').modal('toggle');
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
        debugger;
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
                    document.getElementById("executiveTable").innerHTML = '';
                    var veh = $('#VehicleNo_hidden').val().split(',');
                    $(veh).each(function (index, item) {
                        AddNewexecutive(item);
                    });
                }
                break;
            case "55":
                $("#localVehicle").hide();
                $("#generalVehicle").show();
                if ($('#VehicleNo_hidden').val() != '') {
                    $('select[id$=lstAssignTo]').empty();
                    $('#lstAssignTo').trigger("chosen:updated");
                    VehicleBind($('#MasterVehicleNo_hidden').val().split(','));
                }
                break;
        }
    }

    function SaveVehicleControlData() {
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
        data += (cddl_State.GetValue() == ',') ? '' : cddl_State.GetValue() + "|";
        data += (cddl_City.GetValue() == ',') ? '' : cddl_City.GetValue() + "|";
        data += (cddl_Pin.GetValue() == ',') ? '' : cddl_Pin.GetValue() + "|";
        data += (cddl_Area.GetValue() == ',') ? '' : cddl_Area.GetValue() + "|";
        data += ctxtPhone.GetText(); // Phone
        var validFlag = Validation();
        if (validFlag == true) {
            // alert(data);
            $("#hfControlSaveData").val(data);
            modalShowHide(0);
        }
        else {
            modalShowHide(1);
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
        cell2.innerHTML = "<button type='button' value='' onclick='AddNewexecutive(0)' class='btn btn-primary btn-xs'><i class='fa fa-plus-circle'></i></button> <button type='button' value='' class='btn btn-danger btn-xs' onclick='removeExecutive(this.parentNode.parentNode)'><i class='fa fa-times-circle'></i></button>";
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

    function SetLocalVehicleNo() {
        // var flag = true;
        var localflag = true;
        var table = document.getElementById("executiveTable");

        document.getElementById('VehicleNo_hidden').value = '';
        var data = '';
        for (var i = 0, row; row = table.rows[i]; i++) {
            for (var j = 0, col; col = row.cells[j]; j++) {
                if (col.children[0].type != 'button') {
                    if (data == '') {

                        if (col.children[0].value == '') {
                            //$("#executiveTable").addClass('error');
                            //$('#hfLocalVehFlag').val(0);
                            jAlert("Vehicle number required");
                            modalShowHide(1);
                            localflag = false;
                            flag = false;
                            return false;
                        }
                        else {
                            data = col.children[0].value;
                            flag = (localflag == false) ? false : true;
                            //if ($('#hfLocalVehFlag').val() != 0) {
                            //    $("#executiveTable").removeClass('error');
                            //    flag = true;
                            //} 
                        }
                    }
                    else {
                        if (col.children[0].value == '') {
                            jAlert("Required");
                            //  return false;
                            flag = false;
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
        return flag;
    }

    function SetGeneralVehicleNo() {

        $('#VehicleNo_hidden').val($("#lstAssignTo").val().join());
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
        if ($("#radioregistercheck").find(":checked").val() == '1') {
            if (ctxtGSTIN1.GetValue() == '' || ctxtGSTIN1.GetValue() == null) {
                flag = false;
                SetMandatory("ctxtGSTIN1");
                jAlert('GSTIN number is Mandatory');
                modalShowHide(1);
            }
            if (ctxtGSTIN2.GetValue() == '' || ctxtGSTIN2.GetValue() == null) {
                flag = false;
                SetMandatory("ctxtGSTIN2");
                jAlert('GSTIN number is Mandatory');
                modalShowHide(1);
            }
            if (ctxtGSTIN3.GetValue() == '' || ctxtGSTIN3.GetValue() == null) {
                flag = false;
                SetMandatory("ctxtGSTIN3");
                jAlert('GSTIN number is Mandatory');
                modalShowHide(1);
            }
        }
        //if ($("#executiveTable").hasClass('error')) {
        //    flag = false;
        //    jAlert("Vehicle number required");

        //}
        return flag;
    }

    function SetMandatory(refAttr) {
        $('#Mandatorytxt_' + refAttr).show();
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
</style>

<button type="button" class="btn btn-primary" data-toggle="modal" data-target="#exampleModal" data-whatever="@mdo" style="height: 35px;">Transporter&#818;</button>

<!-- Modal -->
<div class="modal fade" id="exampleModal" role="dialog" aria-labelledby="exampleModalLabel" data-backdrop="static">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" id="exampleModalLabel">Transport Details</h4>
            </div>
            <div class="modal-body">
                <form>
                    <dxe:ASPxCallbackPanel runat="server" ID="callBackuserControlPanelMain" ClientInstanceName="ccallBackuserControlPanelMain" OnCallback="ComponentPanelMain_Callback">
                        <panelcollection>
                                           <dxe:PanelContent runat="server">
                    <div class="col-md-3">
                        <div class="form-group">
                            <label for="recipient-name" class="control-label">Transporter Name</label>
                            <dxe:ASPxComboBox ID="cmbTransporter" runat="server" ClientInstanceName="cddl_Transporter" Width="100%">
                                <clientsideevents selectedindexchanged="function(s, e) { PopulateVehicleDetails(e)}" />
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                    <div class="clear"></div>
                    <%--<div class="clear"></div>--%>
                    <div id="vehicleDeptCtrl" style="display: none;" class="row">
                        <div class="col-md-12">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label for="recipient-name" class="control-label">Transporter Type</label>
                                    <dxe:ASPxComboBox ID="cmbTransporterType" runat="server" ClientInstanceName="cddl_TransporterType" onCallback="cmbTransporterType_Callback"
                                        Width="100%">
                                        <clientsideevents endcallback="cmbTransporterType_EndCallBack" selectedindexchanged="cmbTransporterType_change" />
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
                                <div class="relative">
                                    <ul class="nestedinput">
                                        <li>
                                            <dxe:ASPxTextBox ID="txtGSTIN1" ClientInstanceName="ctxtGSTIN1" MaxLength="2" runat="server" Width="33px">
                                                <clientsideevents keypress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                            </dxe:ASPxTextBox>
                                            <span id="Mandatorytxt_txtGSTIN1" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                                        </li>
                                        <li class="dash">-</li>
                                        <li>
                                            <dxe:ASPxTextBox ID="txtGSTIN2" ClientInstanceName="ctxtGSTIN2" MaxLength="10" runat="server" Width="90px">
                                                <clientsideevents keyup="Gstin2TextChanged" />
                                            </dxe:ASPxTextBox>
                                        </li>
                                        <li class="dash">-</li>
                                        <li>
                                            <dxe:ASPxTextBox ID="txtGSTIN3" ClientInstanceName="ctxtGSTIN3" MaxLength="3" runat="server" Width="90px">
                                                <clientsideevents />
                                            </dxe:ASPxTextBox>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <div class="clear"></div>
                            <div class="col-md-5">
                                <div class="form-group">
                                    <div id="localVehicle" style="display: none">
                                        <%-- Start Tranporter Vechile No --%>
                                        <asp:Panel ID="pnlVehicleNo" runat="server" CssClass="">
                                            <label style="display: block;">Vehicle No.</label>
                                            <table id="executiveTable" style="width: 320px;" class="nbackBtn" runat="server">
                                                <tr>
                                                    <td style="padding-right: 15px">
                                                        <input type="text" maxlength="18" />
                                                    </td>
                                                    <td>
                                                        <button type="button" class="btn btn-primary btn-xs" onclick="AddNewexecutive('0')"><i class="fa fa-plus-circle"></i></button>
                                                        <button type="button" class="btn btn-danger btn-xs" onclick="removeExecutive(this.parentNode.parentNode)"><i class="fa fa-times-circle"></i></button>
                                                    </td>

                                                </tr>
                                            </table>
                                            <asp:HiddenField ID="hfLocalVehFlag" runat="server" />
                                            <asp:HiddenField ID="MasterVehicleNo_hidden" runat="server" />
                                            <asp:HiddenField ID="VehicleNo_hidden" runat="server" />
                                        </asp:Panel>
                                        <%-- END Tranporter Vechile No --%>
                                    </div>

                                    <div id="generalVehicle" style="display: none">
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

                            <div class="col-md-12">
                                <div class="form-group">
                                    <label for="recipient-name" class="control-label">Address</label>
                                    <dxe:ASPxTextBox ID="txtAddress" MaxLength="80" ClientInstanceName="ctxtAddress"
                                        runat="server" Width="100%">
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                             <dxe:ASPxCallbackPanel runat="server" ID="callBackuserControlPanel" ClientInstanceName="ccallBackuserControlPanel" OnCallback="ComponentPanel_Callback">
                        <panelcollection>
                                           <dxe:PanelContent runat="server">
                            
                                            <div class="clear"></div>
                                           <div class="col-md-4">
                                               <div class="form-group">
                                                    <label for="recipient-name" class="control-label">Country</label>
                                                    <dxe:ASPxComboBox ID="vcCmbCountry" ClientInstanceName="cddl_Country" runat="server"  Width="100%">
                                                        <clearbutton displaymode="Always"></clearbutton>
                                                        <clientsideevents selectedindexchanged="function(s, e) { UcOnCountryChanged(s); }"></clientsideevents>
                                                    </dxe:ASPxComboBox>
                                                </div>
                                           </div>
                                           <div class="col-md-4">
                                                <div class="form-group">
                                                    <label for="recipient-name" class="control-label">State</label>
                                                    <div>
                                                        <dxe:ASPxComboBox ID="vcCmbState" ClientInstanceName="cddl_State" runat="server" 
                                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0"
                                                            OnCallback="cmbState_OnCallback">
                                                            <clearbutton displaymode="Always"></clearbutton>
                                                            <clientsideevents selectedindexchanged="function(s, e) { UcOnStateChanged(s); }"></clientsideevents>
                                                        </dxe:ASPxComboBox>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label for="recipient-name" class="control-label">City/District</label>
                                                    <div>
                                                        <dxe:ASPxComboBox ID="vcCmbCity" ClientInstanceName="cddl_City" runat="server" 
                                                            OnCallback="cmbCity_OnCallback" Width="100%">
                                                            <clearbutton displaymode="Always"></clearbutton>
                                                            <clientsideevents selectedindexchanged="function(s, e) { UcOnCityChanged(s); }"></clientsideevents>
                                                        </dxe:ASPxComboBox>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label for="recipient-name" class="control-label">Pin/Zip</label>
                                                    <div>
                                                        <dxe:ASPxComboBox ID="vcCmbPin" ClientInstanceName="cddl_Pin" runat="server" 
                                                            OnCallback="cmbPin_OnCallback" Width="100%">
                                                            <clearbutton displaymode="Always"></clearbutton>
                                                        </dxe:ASPxComboBox>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label for="recipient-name" class="control-label">Area</label>
                                                    <div>
                                                        <dxe:ASPxComboBox ID="vcCmbArea" ClientInstanceName="cddl_Area" runat="server" 
                                                            OnCallback="cmbArea_OnCallback" Width="100%">
                                                            <clearbutton displaymode="Always"></clearbutton>
                                                        </dxe:ASPxComboBox>
                                                       
                                                    </div>
                                                </div>
                                            </div>
                                                </dxe:PanelContent>
                                          </panelcollection>
                        <%--<clientsideevents endcallback="ccallBackuserControlPanel_EndCallback"/>--%>
                    </dxe:ASPxCallbackPanel>
                                            
            <div class="col-md-4">
                        <div class="form-group">
                            <label for="recipient-name" class="control-label">Phone No</label>
                            <dxe:ASPxTextBox ID="txtPhone" MaxLength="80" ClientInstanceName="ctxtPhone"
                                runat="server" Width="100%">
                            </dxe:ASPxTextBox>
                        </div>
                    </div>
            </div>
            <div class="clear"></div>
             </dxe:PanelContent>
                                          </panelcollection>
                        <clientsideevents endcallback="ccallBackuserControlPanel_EndCallback" />
                    </dxe:ASPxCallbackPanel>
            </div>
            <div class="modal-footer" style="text-align: center !important">
                <button type="button" class="btn btn-primary" onclick="SaveVehicleControlData()" style="margin-bottom: 0 !important"><u>S</u>ave</button>
                <button type="button" class="btn btn-danger" data-dismiss="modal"><u>C</u>ancel</button>
            </div>
            </form>

        </div>


    </div>
</div>
</div>
<asp:HiddenField ID="hfControlSaveData" runat="server" />



