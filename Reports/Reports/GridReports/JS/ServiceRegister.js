
        $(document).ready(function () {
            setTimeout(function () {
                $('.multi').multiselect({
                    //  includeSelectAllOption: true,
                    // enableFiltering:true,
                    maxHeight: 200,
                    buttonText: function (options) {
                        if (options.length == 0) {
                            return 'None selected';
                        } else {
                            var selected = 0;
                            options.each(function () {
                                selected += 1;
                            });
                            return selected + ' Selected ';
                        }
                    }
                });
            }, 1000);

            $(".date").datepicker({
                autoclose: true,
                todayHighlight: true,
                format: 'dd-mm-yyyy'
            });//.datepicker('update', new Date())
        });

$(function () {
    cEntityCodePanel.PerformCallback('BindEntityCodeGrid' + '~' + "All");
    cModelPanel.PerformCallback('BindModelGrid' + '~' + "All");
    cProblemFoundPanel.PerformCallback('BindProblemFoundGrid' + '~' + "All");
    cTechnicianPanel.PerformCallback('BindTechnicianGrid' + '~' + "All");
    cLocationPanel.PerformCallback('BindLocationGrid' + '~' + "All");
    //  cProblemReportedPanel.SetEnabled(false);
});

    $(document).ready(function () {
       
        cModelPanel.SetEnabled(false);
        cProblemFoundPanel.SetEnabled(false);
    });

function ddlReport_change() {
    $("#ddlBillable").val(2);
    if ($("#ddlReport").val() != null) {
        //rev Pratik
        //if ($("#ddlReport").val().includes("Repaired") == true || $("#ddlReport").val().includes("Upgradation") == true || $("#ddlReport").val().includes("Degradation") == true || $("#ddlReport").val().includes("Exchanged") == true
        //            || $("#ddlReport").val().includes("Returned") == true || $("#ddlReport").val().includes("ServiceHistory") == true) {
        if ($("#ddlReport").val().includes("Repaired") == true || $("#ddlReport").val().includes("Upgradation") == true || $("#ddlReport").val().includes("Degradation") == true || $("#ddlReport").val().includes("Exchanged") == true
                    || $("#ddlReport").val().includes("Returned") == true || $("#ddlReport").val().includes("ServiceHistory") == true || $("#ddlReport").val().includes("PNF") == true) {
            //End of rev Pratik
            //document.getElementById("ddlProblem").disabled = false;
            //$('#ddlProblem').multiselect('enable');
            //$('#ddlModel').multiselect('enable');
            //document.getElementById("ddlModel").disabled = false;
            cModelPanel.SetEnabled(true);
            cProblemFoundPanel.SetEnabled(true);
            document.getElementById("ddlBillable").disabled = false;

        }
        else {
            //$('#ddlModel').val('').multiselect('refresh');
            //$('#ddlProblem').val('').multiselect('refresh');

            //$('#ddlProblem').multiselect('disable');
            //$('#ddlModel').multiselect('disable');
            gridModelLookup.gridView.UnselectRows();
            gridProblemFoundLookup.gridView.UnselectRows();
            cModelPanel.SetEnabled(false);
            cProblemFoundPanel.SetEnabled(false);
            document.getElementById("ddlBillable").disabled = true;
        }
    }
    else {
        //$('#ddlModel').val('').multiselect('refresh');
        //$('#ddlProblem').val('').multiselect('refresh');

        //$('#ddlProblem').multiselect('disable');
        //$('#ddlModel').multiselect('disable');
        gridModelLookup.gridView.UnselectRows();
        gridProblemFoundLookup.gridView.UnselectRows();
        cModelPanel.SetEnabled(false);
        cProblemFoundPanel.SetEnabled(false);
        document.getElementById("ddlBillable").disabled = true;
    }
}

function ddlType_change() {
    // debugger;
    var entry = "";
    entry = entry + ' <select id="ddlEntityType" class="multi" multiple="multiple">';


    var Report = '';
    Report = Report + ' <select id="ddlReport" class="multi" multiple="multiple"  onchange="ddlReport_change();"> ';
    if ($("#ddlType").val() != null) {
        if ($("#ddlType").val().includes("rcptChallan")) {
            Report = Report + '  <option value="P">Receipt</option>';
            Report = Report + '  <option value="DU">Open Job</option>';
            document.getElementById("chkProblemReport").disabled = false;

            entry = entry + '<option value="1">Token</option> ';
            entry = entry + '<option value="2">Challan</option>';
            entry = entry + '<option value="3">Worksheet</option>';
        }
        else if ($("#ddlType").val().includes("Service")) {
            Report = Report + '  <option value="Repaired">Repaired</option>';
            Report = Report + '  <option value="Upgradation">Upgradation</option>';
            Report = Report + '  <option value="Degradation">Degradation</option>';
            Report = Report + '  <option value="Exchanged">Exchanged</option>';
            Report = Report + '  <option value="Returned">Returned</option>';
            Report = Report + '  <option value="ServiceHistory">Service History</option>';
            //rev Pratik
            Report = Report + '  <option value="PNF">PNF</option>';
            //End of rev Pratik
            document.getElementById("chkProblemReport").disabled = true;

            entry = entry + '<option value="1">Token</option> ';
            entry = entry + '<option value="2">Challan</option>';
            entry = entry + '<option value="3">Worksheet</option>';
            entry = entry + '<option value="4">JobSheet</option>';
        }
        else if ($("#ddlType").val().includes("Delivery")) {
            Report = Report + '  <option value="DE">Delivered</option>';
            Report = Report + '  <option value="DN">Undelivered</option>';
            document.getElementById("chkProblemReport").disabled = false;

            entry = entry + '<option value="1">Token</option> ';
            entry = entry + '<option value="2">Challan</option>';
            entry = entry + '<option value="3">Worksheet</option>';
            entry = entry + '<option value="4">JobSheet</option>';
        }
    }
    else {
        $('#ddlModel').val('').multiselect('refresh');
        $('#ddlProblem').val('').multiselect('refresh');

        $('#ddlProblem').multiselect('disable');
        $('#ddlModel').multiselect('disable');
    }
    Report = Report + ' </select>';

    entry = entry + '</select>';

    $('#DivEntityType').html(entry);

    $('#divReport').html(Report);
    $('.multi').multiselect({
        // includeSelectAllOption: true,
        //  enableFiltering: true,
        maxHeight: 200,
        buttonText: function (options) {
            if (options.length == 0) {
                return 'None selected';
            } else {
                var selected = 0;
                options.each(function () {
                    selected += 1;
                });
                return selected + ' Selected ';
            }
        }
    });

}

function Generate_Report() {
    if ($("#ddlType").val() != "") {
        if ($("#ddlReport").val() != null) {
            if ($("#ddlType").val() == "Delivery" || $("#ddlType").val() == "rcptChallan") {
                if ($("#ddlReport").val().includes("P") == true || $("#ddlReport").val().includes("DU") == true || $("#ddlReport").val().includes("DE") == true || $("#ddlReport").val().includes("DN") == true) {
                    //if ($("#ddlType").val() != "Delivery") {
                    if ($('#chkProblemReport').is(':checked') == true) {
                       
                        $("#DivServiceRegisterReportEntry").addClass("hide");
                        $("#DivServiceRegisterReportSummary").addClass("hide");
                        $("#DivServiceRegisterReportDetails").removeClass("hide");
                        $("#hdnGridTytpe").val("Details");
                        //Grid.Refresh();
                        ReceiptChalanDetails();
                    }
                    else {
                        
                        $("#DivServiceRegisterReportEntry").addClass("hide");
                        $("#DivServiceRegisterReportDetails").addClass("hide");
                        $("#DivServiceRegisterReportSummary").removeClass("hide");
                        $("#hdnGridTytpe").val("Summary");
                       // GridHeader.Refresh();
                        ReceiptChalan();
                    }
                    //}
                    //else {
                    //    ReceiptChalan();
                    //}
                }
                else if ($("#ddlReport").val().includes("Repaired") == true || $("#ddlReport").val().includes("Upgradation") == true || $("#ddlReport").val().includes("Degradation") == true || $("#ddlReport").val().includes("Exchanged") == true
                    || $("#ddlReport").val().includes("Returned") == true || $("#ddlReport").val().includes("ServiceHistory") == true) {
                   
                    $("#DivServiceRegisterReportDetails").addClass("hide");
                    $("#DivServiceRegisterReportSummary").addClass("hide");
                    $("#DivServiceRegisterReportEntry").removeClass("hide");
                    $("#hdnGridTytpe").val("ServiceEntry");
                    //Grid1.Refresh();
                    DeliveryReport();
                }
            }
            else if ($("#ddlType").val() == "Service") {
              
                $("#DivServiceRegisterReportDetails").addClass("hide");
                $("#DivServiceRegisterReportSummary").addClass("hide");
                $("#DivServiceRegisterReportEntry").removeClass("hide");
                $("#hdnGridTytpe").val("ServiceEntry");
                //Grid1.Refresh();
                DeliveryReport();
            }
        }
        else {
            jAlert("Please select Report.");
            $("#ddlReport").focus();
        }
    }
    else {
        jAlert("Please select Type.");
        $("#ddlType").focus();
    }
}


function ReceiptChalan() {
    LoadingPanel.Show();
    var LocationID = "";
    var Locations = "";

    var Tech = "";
    var Technician = "";

    var Prob = "";
    var ProblemId = "";

    var Enitity = "";
    var EnitityId = "";

    var Model = "";
    var ModelId = "";
    
    var ProblemReportId = "";

    var IsProblemReport = "No";
    if ($('#chkProblemReport').is(':checked') == true) {
        IsProblemReport = "Yes";
    }

    gridLocationLookup.gridView.GetSelectedFieldValues("ID", function (val) {
        LocationID = val

        for (var i = 0; i < LocationID.length; i++) {
            if (Locations == "") {
                Locations = LocationID[i];
            }
            else {
                Locations += ',' + LocationID[i];
            }
        }

        gridTechnicianLookup.gridView.GetSelectedFieldValues("cnt_internalId", function (val) {
            Tech = val

            for (var i = 0; i < Tech.length; i++) {
                if (Technician == "") {
                    Technician = Tech[i];
                }
                else {
                    Technician += ',' + Tech[i];
                }
            }

            gridProblemFoundLookup.gridView.GetSelectedFieldValues("ProblemID", function (val) {
                Prob = val

                for (var i = 0; i < Prob.length; i++) {
                    if (ProblemId == "") {
                        ProblemId = Prob[i];
                    }
                    else {
                        ProblemId += ',' + Prob[i];
                    }
                }

                gridEntityCodeLookup.gridView.GetSelectedFieldValues("ID", function (val) {
                    Enitity = val

                    for (var i = 0; i < Enitity.length; i++) {
                        if (EnitityId == "") {
                            EnitityId = Enitity[i];
                        }
                        else {
                            EnitityId += ',' + Enitity[i];
                        }
                    }

                    gridModelLookup.gridView.GetSelectedFieldValues("ID", function (val) {
                        Model = val

                        for (var i = 0; i < Model.length; i++) {
                            if (ModelId == "") {
                                ModelId = Model[i];
                            }
                            else {
                                ModelId += ',' + Model[i];
                            }
                        }


                        var Data = {
                            Type: $("#ddlType").val(),
                            Report: $("#ddlReport").val(),
                            FromDate: $("#dtFromdate").val(),
                            ToDate: $("#dtToDate").val(),
                            EntityCode: EnitityId,
                            EntryType: $("#ddlEntityType").val(),
                            Model: ModelId,
                            ProblemFound: ProblemId,
                            Technician: Technician,
                            Location: Locations,
                            IsBillable: $("#ddlBillable").val(),
                            ProblemReported: ProblemReportId,
                            IsProbLemReport: IsProblemReport,
                            IsDelivery: $("#ddlConfirmDelivered").val()
                        }


                        $.ajax({
                            type: "POST",
                            url: "SRVServiceRegisterReport.aspx/ReceiptChalan",
                            data: JSON.stringify({ Data: Data }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: function (msg) {
                                LoadingPanel.Hide();
                                GridHeader.Refresh();
                            }
                        });
                    });
                });
            });
        });
    });
}



function ReceiptChalanDetails() {
    LoadingPanel.Show();
    var LocationID = "";//gridLocationLookup.gridView.GetSelectedKeysOnPage();
    var Locations = "";

    var Tech = "";//gridTechnicianLookup.gridView.GetSelectedKeysOnPage();
    var Technician = "";

    var Prob = "";//gridProblemFoundLookup.gridView.GetSelectedKeysOnPage();
    var ProblemId = "";

    var Enitity = "";//gridEntityCodeLookup.gridView.GetSelectedKeysOnPage();
    var EnitityId = "";

    var Model = "";//gridModelLookup.gridView.GetSelectedKeysOnPage();
    var ModelId = "";

    // var ProbReport = gridProblemReportedLookup.gridView.GetSelectedKeysOnPage();
    var ProblemReportId = "";

    var IsProblemReport = "No";
    if ($('#chkProblemReport').is(':checked') == true) {
        IsProblemReport = "Yes";
    }

    gridLocationLookup.gridView.GetSelectedFieldValues("ID", function (val) {
        LocationID = val

        for (var i = 0; i < LocationID.length; i++) {
            if (Locations == "") {
                Locations = LocationID[i];
            }
            else {
                Locations += ',' + LocationID[i];
            }
        }

        gridTechnicianLookup.gridView.GetSelectedFieldValues("cnt_internalId", function (val) {
            Tech = val

            for (var i = 0; i < Tech.length; i++) {
                if (Technician == "") {
                    Technician = Tech[i];
                }
                else {
                    Technician += ',' + Tech[i];
                }
            }

            gridProblemFoundLookup.gridView.GetSelectedFieldValues("ProblemID", function (val) {
                Prob = val

                for (var i = 0; i < Prob.length; i++) {
                    if (ProblemId == "") {
                        ProblemId = Prob[i];
                    }
                    else {
                        ProblemId += ',' + Prob[i];
                    }
                }

                gridEntityCodeLookup.gridView.GetSelectedFieldValues("ID", function (val) {
                    Enitity = val

                    for (var i = 0; i < Enitity.length; i++) {
                        if (EnitityId == "") {
                            EnitityId = Enitity[i];
                        }
                        else {
                            EnitityId += ',' + Enitity[i];
                        }
                    }

                    gridModelLookup.gridView.GetSelectedFieldValues("ID", function (val) {
                        Model = val

                        for (var i = 0; i < Model.length; i++) {
                            if (ModelId == "") {
                                ModelId = Model[i];
                            }
                            else {
                                ModelId += ',' + Model[i];
                            }
                        }
                        var Data = {
                            Type: $("#ddlType").val(),
                            Report: $("#ddlReport").val(),
                            FromDate: $("#dtFromdate").val(),
                            ToDate: $("#dtToDate").val(),
                            EntityCode: EnitityId,
                            EntryType: $("#ddlEntityType").val(),
                            Model: ModelId,
                            ProblemFound: ProblemId,
                            Technician: Technician,
                            Location: Locations,
                            IsBillable: $("#ddlBillable").val(),
                            ProblemReported: ProblemReportId,
                            IsProbLemReport: IsProblemReport,
                            IsDelivery: $("#ddlConfirmDelivered").val()
                        }


                        $.ajax({
                            type: "POST",
                            url: "SRVServiceRegisterReport.aspx/ReceiptChalanDetails",
                            data: JSON.stringify({ Data: Data }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async:false,
                            success: function (msg) {
                                LoadingPanel.Hide();
                                Grid.Refresh();
                            }
                        });
                    });
                });
            });
        });
    });
}



function DeliveryReport() {
    LoadingPanel.Show();
    var LocationID = "";//gridLocationLookup.gridView.GetSelectedKeysOnPage();
    var Locations = "";

    var Tech = "";//gridTechnicianLookup.gridView.GetSelectedKeysOnPage();
    var Technician = "";

    var Prob = "";//gridProblemFoundLookup.gridView.GetSelectedKeysOnPage();
    var ProblemId = "";

    var Enitity = "";//gridEntityCodeLookup.gridView.GetSelectedKeysOnPage();
    var EnitityId = "";

    var Model = "";//gridModelLookup.gridView.GetSelectedKeysOnPage();
    var ModelId = "";

    // var ProbReport = gridProblemReportedLookup.gridView.GetSelectedKeysOnPage();
    var ProblemReportId = "";

    var IsProblemReport = "No";
    if ($('#chkProblemReport').is(':checked') == true) {
        IsProblemReport = "Yes";
    }

    gridLocationLookup.gridView.GetSelectedFieldValues("ID", function (val) {
        LocationID = val

        for (var i = 0; i < LocationID.length; i++) {
            if (Locations == "") {
                Locations = LocationID[i];
            }
            else {
                Locations += ',' + LocationID[i];
            }
        }

        gridTechnicianLookup.gridView.GetSelectedFieldValues("cnt_internalId", function (val) {
            Tech = val

            for (var i = 0; i < Tech.length; i++) {
                if (Technician == "") {
                    Technician = Tech[i];
                }
                else {
                    Technician += ',' + Tech[i];
                }
            }

            gridProblemFoundLookup.gridView.GetSelectedFieldValues("ProblemID", function (val) {
                Prob = val

                for (var i = 0; i < Prob.length; i++) {
                    if (ProblemId == "") {
                        ProblemId = Prob[i];
                    }
                    else {
                        ProblemId += ',' + Prob[i];
                    }
                }

                gridEntityCodeLookup.gridView.GetSelectedFieldValues("ID", function (val) {
                    Enitity = val

                    for (var i = 0; i < Enitity.length; i++) {
                        if (EnitityId == "") {
                            EnitityId = Enitity[i];
                        }
                        else {
                            EnitityId += ',' + Enitity[i];
                        }
                    }

                    gridModelLookup.gridView.GetSelectedFieldValues("ID", function (val) {
                        Model = val

                        for (var i = 0; i < Model.length; i++) {
                            if (ModelId == "") {
                                ModelId = Model[i];
                            }
                            else {
                                ModelId += ',' + Model[i];
                            }
                        }

                        var Data = {
                            Type: $("#ddlType").val(),
                            Report: $("#ddlReport").val(),
                            FromDate: $("#dtFromdate").val(),
                            ToDate: $("#dtToDate").val(),
                            EntityCode: EnitityId,
                            EntryType: $("#ddlEntityType").val(),
                            Model: ModelId,
                            ProblemFound: ProblemId,
                            Technician: Technician,
                            Location: Locations,
                            IsBillable: $("#ddlBillable").val(),
                            ProblemReported: ProblemReportId,
                            IsProbLemReport: IsProblemReport,
                            IsDelivery: $("#ddlConfirmDelivered").val()
                        }


                        $.ajax({
                            type: "POST",
                            url: "SRVServiceRegisterReport.aspx/DeliveryReport",
                            data: JSON.stringify({ Data: Data }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: function (msg) {
                                LoadingPanel.Hide();
                                Grid1.Refresh();
                            }
                        });
                    });
                });
            });
        });
    });
}

function ExportChange() {
    if ($("#drdExport").val() == "1") {
        $('#dataTable').DataTable().button('.buttons-pdf').trigger();
    }
    if ($("#drdExport").val() == "2") {
        $('#dataTable').DataTable().button('.buttons-excel').trigger();
    }
    if ($("#drdExport").val() == "4") {
        $('#dataTable').DataTable().button('.buttons-csv').trigger();
    }
}


    function selectAllEntityCode() {
        gridEntityCodeLookup.gridView.SelectRows();
    }
function unselectAllEntityCode() {
    gridEntityCodeLookup.gridView.UnselectRows();
}
function CloseEntityCodeLookup() {
    gridEntityCodeLookup.ConfirmCurrentSelection();
    gridEntityCodeLookup.HideDropDown();
    gridEntityCodeLookup.Focus();
}

function selectAllModel() {
    gridModelLookup.gridView.SelectRows();
}
function unselectAllModel() {
    gridModelLookup.gridView.UnselectRows();
}
function CloseModelLookup() {
    gridModelLookup.ConfirmCurrentSelection();
    gridModelLookup.HideDropDown();
    gridModelLookup.Focus();
}

function selectAllProblem() {
    gridProblemFoundLookup.gridView.SelectRows();
}
function unselectAllProblem() {
    gridProblemFoundLookup.gridView.UnselectRows();
}
function CloseProblemLookup() {
    gridProblemFoundLookup.ConfirmCurrentSelection();
    gridProblemFoundLookup.HideDropDown();
    gridProblemFoundLookup.Focus();
}

function selectAllTechnician() {
    gridTechnicianLookup.gridView.SelectRows();
}
function unselectAllTechnician() {
    gridTechnicianLookup.gridView.UnselectRows();
}
function CloseTechnicianLookup() {
    gridTechnicianLookup.ConfirmCurrentSelection();
    gridTechnicianLookup.HideDropDown();
    gridTechnicianLookup.Focus();
}

function selectAllLocation() {
    gridLocationLookup.gridView.SelectRows();
}
function unselectAllLocation() {
    gridLocationLookup.gridView.UnselectRows();
}
function CloseLoactionLookup() {
    gridLocationLookup.ConfirmCurrentSelection();
    gridLocationLookup.HideDropDown();
    gridLocationLookup.Focus();
}

function chkProblemReport_change() {
    if ($('#chkProblemReport').is(':checked') == true) {
        //cProblemFoundPanel.PerformCallback('BindProblemReportedGrid' + '~' + "All");
        //cProblemReportedPanel.SetEnabled(true);
    }
    else {
        //gridProblemReportedLookup.gridView.UnselectRows();
        //cProblemReportedPanel.SetEnabled(false);
    }
}


        function Callback_EndCallback() {
            $("#drdExport").val(0);
        }

function Grid1Callback_EndCallback() {

}

function HeaderCallback_EndCallback() {

}
