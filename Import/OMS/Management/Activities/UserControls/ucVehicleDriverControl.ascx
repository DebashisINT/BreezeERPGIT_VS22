<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucVehicleDriverControl.ascx.cs" Inherits="ERP.OMS.Management.Activities.UserControls.ucVehicleDriverControl" %>

<script src="/assests/pluggins/choosen/choosen.min.js"></script>
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

    #LBDriver {
        width: 200px;
    }
</style>
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

    .btn {
        margin-bottom: 0 !important;
        margin-top: 8px;
    }
</style>
<script>

    var url = getUrl();
    $(document).ready(function () {
        if (url.key != 'ADD') {
            if (VDcddl_Transporter.GetValue() != '' && VDcddl_Transporter.GetValue() != null) {
                $("#VehicleDriverCtrl").show();
                SaveVehicleDriverData();
            }
        }
        ListVDBind();
        ListvehicleDriver();

    });
    function VDccallBackuserControlPanel_EndCallback() {
        jAlert('Saved Successfully.', 'Alert Message!!!');
        VDmodalShowHide(0);
        clearVehicleDriver();
    }
    function getUrl() {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    }
    function ListVDBind() {
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
    function ListvehicleDriver() {

        $('#LBDriver').chosen();
        $('#LBDriver').fadeIn();
    }
    function DriverBind(ObjVehiclesDriver, ObjAllDrivers) {
        var lAssignTo = $('select[id$=LBDriver]');
        lAssignTo.empty();
        $('#LBDriver').trigger("chosen:updated");
        var listItems = [];
        $(ObjAllDrivers).each(function (index, item) {

            listItems.push('<option value="' +
               item.DriversInternalID + '" select>' + item.fullname
               + '</option>');
        });

        $(lAssignTo).append(listItems.join(''));
        ListvehicleDriver();
        var lstTaxRates_Ctrl = document.getElementById("LBDriver");

        $(ObjVehiclesDriver).each(function (index, item) {

            for (var i = 0; i < lstTaxRates_Ctrl.options.length; i++) {
                if (lstTaxRates_Ctrl.options[i].value == item.DriversInternalID) {
                    lstTaxRates_Ctrl.options[i].selected = true;
                }
            }
        });

        $('#LBDriver').trigger("chosen:updated");
    }
    function VDmodalShowHide(param) {

        switch (param) {
            case 0:
                $('#VehicleDriverModal').modal('toggle');
                break;
            case 1:
                $('#VehicleDriverModal').modal({
                    show: 'true'
                });
                break;
        }

    }
    function PopulateVehicleDriverDetails(e) {

        VDcddl_TransporterType.PerformCallback(VDcddl_Transporter.GetValue());
        //registeredCheckChangeEvent();
    }
    function VDcmbTransporterType_EndCallBack() {

        if (VDcddl_TransporterType.cpVDBind != '') {
            var vehicleDetails = VDcddl_TransporterType.cpVDBind;

            $("#txtvehicle_regNo").val(vehicleDetails.vehicle_regNo);
            $("#txtvehicle_engineNo").val(vehicleDetails.vehicle_engineNo);
            $("#txtvehicle_Type").val(vehicleDetails.vehicle_Type);
            $("#txtvehicle_maker").val(vehicleDetails.vehicle_maker);
            $("#txtvehicle_model").val(vehicleDetails.vehicle_model);
            $("#txtvehicle_yearReg").val(vehicleDetails.vehicle_yearReg);
            $("#txtvehicle_fuelType").val(vehicleDetails.vehicle_fuelType);
            $("#txtvehicle_Pollution").val(vehicleDetails.vehicle_Pollution);
            $("#txtvehicle_isGPSInstalled").val(vehicleDetails.vehicle_isGPSInstalled);
            $("#txtvehicle_BlueBook").val(vehicleDetails.vehicle_BlueBook);
            $("#txtvehicle_engineCC").val(vehicleDetails.vehicle_engineCC);
            $("#txtvehicle_AllotedTo").val(vehicleDetails.vehicle_AllotedTo);
            $("#txtvehicle_FleetCardNumber").val(vehicleDetails.vehicle_FleetCardNumber);
            $("#txtvehicle_HappyCard").val(vehicleDetails.vehicle_HappyCard);
            $("#txtvehicle_InsurerName").val(vehicleDetails.vehicle_InsurerName);
            $("#txtvehicle_PolicyNo").val(vehicleDetails.vehicle_PolicyNo);
            $("#txtvehicle_PolicyValidUpto").val(vehicleDetails.vehicle_PolicyValidUpto);
            $("#txtvehicle_InsuranceGivenTo").val(vehicleDetails.vehicle_InsuranceGivenTo);
            $("#txtvehicle_TaxTokenNo").val(vehicleDetails.vehicle_TaxTokenNo);
            $("#txtvehicle_TaxValidUpto").val(vehicleDetails.vehicle_TaxValidUpto);
            $("#txtvehicle_PollutionCaseDtl").val(vehicleDetails.vehicle_PollutionCaseDtl);
            $("#vtxtehicle_PollutionCertValidUpto").val(vehicleDetails.vehicle_PollutionCertValidUpto);
            $("#txtvehicle_AuthLetterValidUpto").val(vehicleDetails.vehicle_AuthLetterValidUpto);
            $("#txtvehicle_CFDetails").val(vehicleDetails.vehicle_CFDetails);
            $("#txtvehicle_CFValidUpto").val(vehicleDetails.vehicle_CFValidUpto);
            $("#txtvehicle_vehOwnerType").val(vehicleDetails.vehicle_vehOwnerType);
            $("#txtvehicle_ChassisNo").val(vehicleDetails.vehicle_ChassisNo);
            $("#txtvehicle_LogBookStatus").val(vehicleDetails.vehicle_LogBookStatus);
            $("#txtvehicle_isFleetCardApplied").val(vehicleDetails.vehicle_isFleetCardApplied);
            $("#txtvehicle_isAuthLetter").val(vehicleDetails.vehicle_isAuthLetter);
            $("#txtvehicle_isActive").val(vehicleDetails.vehicle_isActive);
            DriverBind(vehicleDetails.VehiclesDriver, vehicleDetails.AllVehiclesDriver);

            $("#divDriver").show();
            $("#VehicleDriverCtrl").show();

            $("#hfVehicleDrivarData").val(vehicleDetails);
            $("#hfDocId").val($("#hfDocId").val());
            $("#hfDocType").val($("#hfDocType").val());
        }
        else {
            $("#divDriver").hide();
            $("#VehicleDriverCtrl").hide();
        }
    }
    function SaveVehicleDriverData() {
        var LBDriver_Ctrl = document.getElementById("LBDriver");
        var DriversID_List = '';

        for (var i = 0; i < LBDriver_Ctrl.options.length; i++) {
            if (LBDriver_Ctrl.options[i].selected == true) {
                DriversID_List += LBDriver_Ctrl.options[i].value + ",";
            }
        }
        if ($("#hfDocId").val() == '') {
            jAlert('Document id is not generated yet.', 'Alert Message!!!');
            VDmodalShowHide(0);
            clearVehicleDriver();
        }
        else if ($("#hfDocType").val() == '') {
            jAlert('Document type is not generated yet.', 'Alert Message!!!');
            VDmodalShowHide(0);
            clearVehicleDriver();
        }
        else if (DriversID_List == '') {
            jAlert('Drivers can not be empty.', 'Alert Message!!!', function () {
                $(".LBDriver").focus();
            });
        }
        else if (VDcddl_Transporter == null || VDcddl_Transporter.GetText() == '') {
            jAlert('vehicle registration no can not be empty.', 'Alert Message!!!');
        }
        else {
            //remove last comma and undefined from DriversID_List
            var DriversID_List = DriversID_List.replace("undefined", "");
            if (DriversID_List.charAt(DriversID_List.length - 1) == ',') {
                DriversID_List = DriversID_List.substr(0, DriversID_List.length - 1);
            }
            //create object to send data
            var DocwiseVehicledriverModel = {
                VehicleRegNo: VDcddl_Transporter.GetText(),
                DocId: $("#hfDocId").val(),
                DocType: $("#hfDocType").val(),
                DriversID: DriversID_List,
                CreatedBy: ""
            }

            cVDcallBackuserControlPanelMain.PerformCallback('SaveVehicleDriverData~' + VDcddl_Transporter.GetText() + '~' + $("#hfDocId").val() + '~' + $("#hfDocType").val() + '~' + DriversID_List);
        }

    }
    function VDcalcelbuttonclick() {
        VDmodalShowHide(0);
        clearVehicleDriver();
    }
    function VDcmbTransporterType_change() {
    }
    function CloseVDModal() {
        $('#VehicleDriverModal').modal({
            show: 'false'
        });
    }
    function clearVehicleDriver() {
        VDcddl_Transporter.SetText('--Select--');
        $('#hfVehicleDrivarData').val('');
        $("#VehicleDriverCtrl").hide();
    }

</script>
<button type="button" class="btn btn-primary" data-toggle="modal" data-target="#VehicleDriverModal" data-whatever="@mdo" style="height: 35px;">Vehicle Details</button>
<!-- Modal -->
<div class="modal fade" id="VehicleDriverModal" role="dialog" aria-labelledby="VehicleDriverModalLabel" data-backdrop="static">
    <div class="modal-dialog " role="document" style="width: 80% !important">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" id="VehicleDriverModalLabel">Vehicle Details</h4>
            </div>
            <div class="modal-body">
                <form>
                    <dxe:ASPxCallbackPanel runat="server" ID="VDcallBackuserControlPanelMain" ClientInstanceName="cVDcallBackuserControlPanelMain" OnCallback="VDComponentPanelMain_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label for="recipient-name" class="control-label">Vehicle Reg No</label>
                                        <dxe:ASPxComboBox ID="VDcmbTransporter" runat="server" ClientInstanceName="VDcddl_Transporter" Width="100%">
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateVehicleDriverDetails(e)}" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>
                                <div class="clear"></div>

                                <div class="row" id="VehicleDriverCtrl" style="display: none;">
                                    <div class="col-md-12">
                                        <div class="col-md-2" style="display: none;">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">Reg No</label>
                                                <input type="text" id="txtvehicle_regNo" disabled title="disabled" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">Engine No</label>
                                                <input type="text" id="txtvehicle_engineNo" disabled title="disabled" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">Type</label>
                                                <input type="text" id="txtvehicle_Type" disabled title="disabled" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">Maker</label>
                                                <input type="text" id="txtvehicle_maker" disabled title="disabled" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">Model</label>
                                                <input type="text" id="txtvehicle_model" disabled title="disabled" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">Year Of Reg</label>
                                                <input type="text" id="txtvehicle_yearReg" disabled title="disabled" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">Fuel Type</label>
                                                <input type="text" id="txtvehicle_fuelType" disabled title="disabled" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">Pollution</label>
                                                <input type="text" id="txtvehicle_Pollution" disabled title="disabled" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">IsGPSInstalled</label>
                                                <input type="text" id="txtvehicle_isGPSInstalled" disabled title="disabled" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">BlueBook</label>
                                                <input type="text" id="txtvehicle_BlueBook" disabled title="disabled" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">EngineCC</label>
                                                <input type="text" id="txtvehicle_engineCC" disabled title="disabled" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">AllotedTo</label>
                                                <input type="text" id="txtvehicle_AllotedTo" disabled title="disabled" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">FleetCardNumber</label>
                                                <input type="text" id="txtvehicle_FleetCardNumber" disabled title="disabled" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">HappyCard</label>
                                                <input type="text" id="txtvehicle_HappyCard" disabled title="disabled" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">Insurer Name</label>
                                                <input type="text" id="txtvehicle_InsurerName" disabled />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">PolicyNo</label>
                                                <input type="text" id="txtvehicle_PolicyNo" disabled title="disabled" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">PolicyValidUpto</label>
                                                <input type="text" id="txtvehicle_PolicyValidUpto" disabled title="disabled" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">InsuranceGivenTo</label>
                                                <input type="text" id="txtvehicle_InsuranceGivenTo" disabled title="disabled" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">TaxTokenNo</label>
                                                <input type="text" id="txtvehicle_TaxTokenNo" disabled title="disabled" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">TaxValidUpto</label>
                                                <input type="text" id="txtvehicle_TaxValidUpto" disabled title="disabled" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">PollutionCaseDtl</label>
                                                <input type="text" id="txtvehicle_PollutionCaseDtl" disabled title="disabled" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">PollutionCertValidUpto</label>
                                                <input type="text" id="txtvehicle_PollutionCertValidUpto" disabled title="disabled" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">AuthLetterValidUpto</label>
                                                <input type="text" id="txtvehicle_AuthLetterValidUpto" disabled title="disabled" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">CFDetails</label>
                                                <input type="text" id="txtvehicle_CFDetails" disabled title="disabled" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">CFValidUpto</label>
                                                <input type="text" id="txtvehicle_CFValidUpto" disabled title="disabled">
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">VehOwnerType</label>
                                                <input type="text" id="txtvehicle_vehOwnerType" disabled title="disabled" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">ChassisNo</label>
                                                <input type="text" id="txtvehicle_ChassisNo" disabled title="disabled" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">LogBookStatus</label>
                                                <input type="text" id="txtvehicle_LogBookStatus" disabled title="disabled" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">IsFleetCardApplied</label>
                                                <input type="text" id="txtvehicle_isFleetCardApplied" disabled title="disabled" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">IsAuthLetter</label>
                                                <input type="text" id="txtvehicle_isAuthLetter" disabled title="disabled" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">IsActive</label>
                                                <input type="text" id="txtvehicle_isActive" disabled title="disabled" />
                                            </div>
                                        </div>

                                        <div class="col-md-8">
                                            <div class="form-group">
                                                <div id="divDriver" style="display: none">
                                                    <label for="recipient-name" class="control-label">Vehicle Driver(s)</label>
                                                    <asp:ListBox ID="LBDriver" SelectionMode="Multiple" CssClass="hide" runat="server" Font-Size="12px" Height="90px" Width="100%" data-placeholder="Select..."></asp:ListBox>
                                                    <asp:Label ID="lblDriver" runat="server" Text=""></asp:Label>
                                                    <span id="MandatoryAssignDriver" style="display: none">
                                                        <span id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EIDriver" class="pullrightClass fa fa-exclamation-circle abs iconRed" title="Mandatory"></span></span>
                                                    <asp:HiddenField ID="HiddenField1Driver" runat="server" />
                                                    <asp:HiddenField ID="HiddenField2Driver" runat="server" />
                                                    <asp:HiddenField ID="HiddenField3Driver" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                        <div class="text-center" style="text-align: center !important">
                                            <button type="button" class="btn btn-primary" onclick="SaveVehicleDriverData()" style="margin-bottom: 0 !important"><u>S</u>ave</button>
                                            <button type="button" class="btn btn-danger" data-dismiss="modal" onclick="VDcalcelbuttonclick()"><u>C</u>ancel</button>
                                        </div>
                                    </div>
                                </div>
                                <div id="vehicleDeptCtrl11" style="display: none;" class="row">
                                    <div class="col-md-12">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">Transporter Type</label>
                                                <dxe:ASPxComboBox ID="VDcmbTransporterType" runat="server" ClientInstanceName="VDcddl_TransporterType" OnCallback="VDcmbTransporterType_Callback"
                                                    Width="100%">
                                                    <ClientSideEvents EndCallback="VDcmbTransporterType_EndCallBack" SelectedIndexChanged="VDcmbTransporterType_change" />
                                                </dxe:ASPxComboBox>
                                            </div>
                                        </div>

                                        <div id="td_registered" class="labelt col-md-2">
                                            <div class="visF">
                                                <label>Registered?</label>
                                                <asp:RadioButtonList runat="server" ID="radioregistercheck111" RepeatDirection="Horizontal" Width="130px">
                                                    <asp:ListItem Text="Yes" Value="1" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>

                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">Address</label>
                                                <dxe:ASPxTextBox ID="txtAddress111" MaxLength="80" ClientInstanceName="ctxtAddress"
                                                    runat="server" Width="100%">
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">Phone No</label>
                                                <dxe:ASPxTextBox ID="txtPhone111" MaxLength="80" ClientInstanceName="ctxtPhone"
                                                    runat="server" Width="100%">
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                        <div class="text-center" style="text-align: center !important">
                                            <button type="button" class="btn btn-primary" style="margin-bottom: 0 !important"><u>S</u>ave</button>
                                            <button type="button" class="btn btn-danger" data-dismiss="modal"><u>C</u>ancel</button>
                                        </div>
                                    </div>
                                </div>
                                <%--Not Required--%>
                                <div class="clear"></div>
                                <asp:HiddenField runat="server" ID="hfVehicleDrivarData" />
                                <asp:HiddenField runat="server" ID="hfDocId" />
                                <asp:HiddenField runat="server" ID="hfDocType" />
                            </dxe:PanelContent>
                        </PanelCollection>
                        <ClientSideEvents EndCallback="VDccallBackuserControlPanel_EndCallback" />
                    </dxe:ASPxCallbackPanel>
                </form>
            </div>

        </div>
    </div>
</div>

