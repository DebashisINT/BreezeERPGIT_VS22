
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
        //$.ajax({
        //    type: "POST",
        //    url: "ServiceRegister.aspx/ServiceRegisterReport",
        //    data: JSON.stringify({ Action: "" }),
        //    contentType: "application/json; charset=utf-8",
        //    dataType: "json",
        //    success: function (msg) {
        //        //var status = '<select id="ddlEntityCode" class="multi" multiple="multiple">';
        //        //for (var i = 0; i < msg.d.EntityCodeList.length; i++) {
        //        //    status = status + '<option value=' + msg.d.EntityCodeList[i].EntityCode + '>' + msg.d.EntityCodeList[i].EntityCode + '</option>';
        //        //}
        //        //status = status + '</select>';

        //        //$("#DivEntityCode").html(status);

        //        //var prob = '<select id="ddlProblem" class="multi" multiple="multiple">';
        //        //for (var i = 0; i < msg.d.ProblemList.length; i++) {
        //        //    prob = prob + '<option value=' + msg.d.ProblemList[i].ProblemID + '>' + msg.d.ProblemList[i].ProblemDesc + '</option>';
        //        //}
        //        //prob = prob + '</select>';

        //        //$("#DivProblem").html(prob);

        //        //var modl = '<select id="ddlModel" class="multi" multiple="multiple">';
        //        //for (var i = 0; i < msg.d.ModelList.length; i++) {
        //        //    modl = modl + '<option value=' + msg.d.ModelList[i].Model + '>' + msg.d.ModelList[i].Model + '</option>';
        //        //}
        //        //modl = modl + '</select>';

        //        //$("#DivModel").html(modl);

        //        //var tech = '<select id="ddlTechnician" class="multi" multiple="multiple">';
        //        //for (var i = 0; i < msg.d.TecchnicianList.length; i++) {
        //        //    tech = tech + '<option value=' + msg.d.TecchnicianList[i].cnt_id + '>' + msg.d.TecchnicianList[i].cnt_firstName + '</option>';
        //        //}
        //        //tech = tech + '</select>';

        //        //$("#DivTechnician").html(tech);

        //        //var brnch = '<select id="ddlLocation" class="multi" multiple="multiple">';
        //        //for (var i = 0; i < msg.d.BranchesList.length; i++) {
        //        //    brnch = brnch + '<option value=' + msg.d.BranchesList[i].branch_id + '>' + msg.d.BranchesList[i].branch_description + '</option>';
        //        //}
        //        //brnch = brnch + '</select>';

        //        //$("#DivLocation").html(brnch);

        //        //$('#ddlProblem').multiselect('disable');
        //        //$('#ddlModel').multiselect('disable');

        //    },
        //    error: function (msg) {
        //    }
        //});
        cModelPanel.SetEnabled(false);
        cProblemFoundPanel.SetEnabled(false);
    });

function ddlReport_change() {
    $("#ddlBillable").val(2);
    if ($("#ddlReport").val() != null) {
        if ($("#ddlReport").val().includes("Repaired") == true || $("#ddlReport").val().includes("Upgradation") == true || $("#ddlReport").val().includes("Degradation") == true || $("#ddlReport").val().includes("Exchanged") == true
                    || $("#ddlReport").val().includes("Returned") == true || $("#ddlReport").val().includes("ServiceHistory") == true) {
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
                        ReceiptChalanDetails();
                    }
                    else {
                        ReceiptChalan();
                    }
                    //}
                    //else {
                    //    ReceiptChalan();
                    //}
                }
                else if ($("#ddlReport").val().includes("Repaired") == true || $("#ddlReport").val().includes("Upgradation") == true || $("#ddlReport").val().includes("Degradation") == true || $("#ddlReport").val().includes("Exchanged") == true
                    || $("#ddlReport").val().includes("Returned") == true || $("#ddlReport").val().includes("ServiceHistory") == true) {
                    DeliveryReport();
                }
            }
            else if ($("#ddlType").val() == "Service") {
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
    var LocationID = "";//gridLocationLookup.gridView.GetSelectedKeysOnPage();
    var Locations = "";
    //for (var i = 0; i < LocationID.length; i++) {
    //    if (Locations == "") {
    //        Locations = LocationID[i];
    //    }
    //    else {
    //        Locations += ',' + LocationID[i];
    //    }
    //}

    var Tech = "";//gridTechnicianLookup.gridView.GetSelectedKeysOnPage();
    var Technician = "";
    //for (var i = 0; i < Tech.length; i++) {
    //    if (Technician == "") {
    //        Technician = Tech[i];
    //    }
    //    else {
    //        Technician += ',' + Tech[i];
    //    }
    //}

    var Prob = "";//gridProblemFoundLookup.gridView.GetSelectedKeysOnPage();
    var ProblemId = "";
    //for (var i = 0; i < Prob.length; i++) {
    //    if (ProblemId == "") {
    //        ProblemId = Prob[i];
    //    }
    //    else {
    //        ProblemId += ',' + Prob[i];
    //    }
    //}

    var Enitity = "";//gridEntityCodeLookup.gridView.GetSelectedKeysOnPage();
    var EnitityId = "";
    //for (var i = 0; i < Enitity.length; i++) {
    //    if (EnitityId == "") {
    //        EnitityId = Enitity[i];
    //    }
    //    else {
    //        EnitityId += ',' + Enitity[i];
    //    }
    //}

    var Model = "";//gridModelLookup.gridView.GetSelectedKeysOnPage();
    var ModelId = "";
    //for (var i = 0; i < Model.length; i++) {
    //    if (ModelId == "") {
    //        ModelId = Model[i];
    //    }
    //    else {
    //        ModelId += ',' + Model[i];
    //    }
    //}

    // var ProbReport = gridProblemReportedLookup.gridView.GetSelectedKeysOnPage();
    var ProblemReportId = "";
    //for (var i = 0; i < ProbReport.length; i++) {
    //    if (ProblemReportId == "") {
    //        ProblemReportId = ProbReport[i];
    //    }
    //    else {
    //        ProblemReportId += ',' + ProbReport[i];
    //    }
    //}
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
                            url: "ServiceRegister.aspx/ReceiptChalan",
                            data: JSON.stringify({ Data: Data }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                var status = "";
                                status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width:100%'>";
                                status = status + " <thead><tr>";
                                status = status + " <th>Receipt Challan</th><th>Date</th><th>Entry Type </th><th>Location </th><th>Entity Code</th><th>Network Name </th>";
                                status = status + " <th>Contact Name </th><th>Contact No </th><th>Received By</th><th>Received On</th><th>Technician</th>";//<th>Problem Reported</th><th>Cord</th><th>Adapter</th>
                                status = status + " <th>Assigned By</th><th>Assigned On</th><th>Serviced By</th><th>Serviced On</th><th>Confirm Delivered</th><th>Delivered To</th>";
                                status = status + " <th>Delivered By</th><th>Delivered On</th><th>Status</th>";
                                status = status + " </tr></thead>";
                                status = status + " </table>";

                                $('#DivServiceRegisterReportDetails').html(status);

                                $('#dataTable').DataTable({
                                    scrollX: true,
                                    fixedColumns: {
                                        rightColumns: 1
                                    },
                                    data: msg.d,
                                    columns: [
                                       { 'data': 'ReceiptChallan' },
                                       { 'data': 'Date' },
                                       { 'data': 'EntryType' },
                                       { 'data': 'Location' },
                                       { 'data': 'EntityCode' },
                                       { 'data': 'NetworkName' },
                                       { 'data': 'ContactName' },
                                       { 'data': 'ContactNo' },
                                       { 'data': 'ReceivedBy' },
                                       { 'data': 'ReceivedOn' },
                                      // { 'data': 'ProblemReported' },
                                      // { 'data': 'Cord' },
                                      // { 'data': 'Adapter' },
                                       { 'data': 'Technician' },
                                       { 'data': 'AssignedBy' },
                                       { 'data': 'AssignedOn' },
                                       { 'data': 'ServicedBy' },
                                       { 'data': 'ServicedOn' },
                                       { 'data': 'ConfirmDelivery' },
                                       { 'data': 'DeliveredTo' },
                                       { 'data': 'DeliveredBy' },
                                       { 'data': 'DeliveredOn' },
                                       { 'data': 'Status' },
                                    ],
                                    dom: 'Bfrtip',
                                    buttons: [
                                        {
                                            extend: 'excel',
                                            title: null,
                                            filename: 'Service Register',
                                            text: 'Save as Excel',
                                            customize: function (xlsx) {
                                                var sheet = xlsx.xl.worksheets['sheet1.xml'];
                                                $('row:first c', sheet).attr('s', '42');
                                            },

                                            exportOptions: {
                                                //columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12,13,14,15,16,17,18,19,20,21,22]
                                            }
                                        }
                                    ],
                                    error: function (error) {
                                        alert(error);
                                    }
                                });
                                LoadingPanel.Hide();
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
    //for (var i = 0; i < LocationID.length; i++) {
    //    if (Locations == "") {
    //        Locations = LocationID[i];
    //    }
    //    else {
    //        Locations += ',' + LocationID[i];
    //    }
    //}

    var Tech = "";//gridTechnicianLookup.gridView.GetSelectedKeysOnPage();
    var Technician = "";
    //for (var i = 0; i < Tech.length; i++) {
    //    if (Technician == "") {
    //        Technician = Tech[i];
    //    }
    //    else {
    //        Technician += ',' + Tech[i];
    //    }
    //}

    var Prob = "";//gridProblemFoundLookup.gridView.GetSelectedKeysOnPage();
    var ProblemId = "";
    //for (var i = 0; i < Prob.length; i++) {
    //    if (ProblemId == "") {
    //        ProblemId = Prob[i];
    //    }
    //    else {
    //        ProblemId += ',' + Prob[i];
    //    }
    //}

    var Enitity = "";//gridEntityCodeLookup.gridView.GetSelectedKeysOnPage();
    var EnitityId = "";
    //for (var i = 0; i < Enitity.length; i++) {
    //    if (EnitityId == "") {
    //        EnitityId = Enitity[i];
    //    }
    //    else {
    //        EnitityId += ',' + Enitity[i];
    //    }
    //}

    var Model = "";//gridModelLookup.gridView.GetSelectedKeysOnPage();
    var ModelId = "";
    //for (var i = 0; i < Model.length; i++) {
    //    if (ModelId == "") {
    //        ModelId = Model[i];
    //    }
    //    else {
    //        ModelId += ',' + Model[i];
    //    }
    //}

    // var ProbReport = gridProblemReportedLookup.gridView.GetSelectedKeysOnPage();
    var ProblemReportId = "";
    //for (var i = 0; i < ProbReport.length; i++) {
    //    if (ProblemReportId == "") {
    //        ProblemReportId = ProbReport[i];
    //    }
    //    else {
    //        ProblemReportId += ',' + ProbReport[i];
    //    }
    //}
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
                            url: "ServiceRegister.aspx/ReceiptChalanDetails",
                            data: JSON.stringify({ Data: Data }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                var status = "";
                                status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width:100%'>";
                                status = status + " <thead><tr>";
                                status = status + " <th>Receipt Challan</th><th>Date</th><th>Entry Type </th><th>Location </th><th>Entity Code</th><th>Network Name </th>";
                                status = status + " <th>Contact Name </th><th>Contact No </th><th>Model No</th><th>Serial No</th><th>Problem Reported</th><th>Received By</th><th>Received On</th>";
                                status = status + " <th>Technician</th><th>Assigned By</th><th>Assigned On</th><th>Service Action</th><th>Component(s)</th><th>Problem Found</th>";
                                status = status + " <th>Problem Remarks</th><th>Billable</th><th>Serviced By</th><th>Serviced On</th>";
                                status = status + " <th>Confirm Delivered</th><th>Delivered To</th><th>Delivered By</th><th>Delivered On</th><th>Status</th>";
                                status = status + " </tr></thead>";
                                status = status + " </table>";

                                $('#DivServiceRegisterReportDetails').html(status);

                                $('#dataTable').DataTable({
                                    scrollX: true,
                                    fixedColumns: {
                                        rightColumns: 1
                                    },
                                    data: msg.d,
                                    columns: [
                                       { 'data': 'ReceiptChallan' },
                                       { 'data': 'Date' },
                                       { 'data': 'EntryType' },
                                       { 'data': 'Location' },
                                       { 'data': 'EntityCode' },
                                       { 'data': 'NetworkName' },
                                       { 'data': 'ContactName' },
                                       { 'data': 'ContactNo' },                                               
                                       { 'data': 'ModelNo' },
                                       { 'data': 'SerialNo' },
                                       { 'data': 'ProblemReported' },
                                       { 'data': 'ReceivedBy' },
                                       { 'data': 'ReceivedOn' },
                                       { 'data': 'Technician' },
                                       { 'data': 'AssignedBy' },
                                       { 'data': 'AssignedOn' },
                                       { 'data': 'ServiceAction' },

                                       { 'data': 'Component' },
                                       { 'data': 'ProblemFound' },
                                       { 'data': 'ProblemRemarks' },
                                       { 'data': 'Billable' },

                                       { 'data': 'ServicedBy' },
                                       { 'data': 'ServicedOn' },
                                       { 'data': 'ConfirmDelivery' },
                                       { 'data': 'DeliveredTo' },
                                       { 'data': 'DeliveredBy' },
                                       { 'data': 'DeliveredOn' },
                                       { 'data': 'Status' },
                                               
                                    ],
                                    dom: 'Bfrtip',
                                    buttons: [
                                        {
                                            extend: 'excel',
                                            title: null,
                                            filename: 'Service Register',
                                            text: 'Save as Excel',
                                            customize: function (xlsx) {
                                                var sheet = xlsx.xl.worksheets['sheet1.xml'];
                                                $('row:first c', sheet).attr('s', '42');
                                            },

                                            exportOptions: {
                                                //columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28]
                                            }
                                        }
                                    ],
                                    error: function (error) {
                                        alert(error);
                                    }
                                });
                                LoadingPanel.Hide();
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
    //for (var i = 0; i < LocationID.length; i++) {
    //    if (Locations == "") {
    //        Locations = LocationID[i];
    //    }
    //    else {
    //        Locations += ',' + LocationID[i];
    //    }
    //}

    var Tech = "";//gridTechnicianLookup.gridView.GetSelectedKeysOnPage();
    var Technician = "";
    //for (var i = 0; i < Tech.length; i++) {
    //    if (Technician == "") {
    //        Technician = Tech[i];
    //    }
    //    else {
    //        Technician += ',' + Tech[i];
    //    }
    //}

    var Prob = "";//gridProblemFoundLookup.gridView.GetSelectedKeysOnPage();
    var ProblemId = "";
    //for (var i = 0; i < Prob.length; i++) {
    //    if (ProblemId == "") {
    //        ProblemId = Prob[i];
    //    }
    //    else {
    //        ProblemId += ',' + Prob[i];
    //    }
    //}

    var Enitity = "";//gridEntityCodeLookup.gridView.GetSelectedKeysOnPage();
    var EnitityId = "";
    //for (var i = 0; i < Enitity.length; i++) {
    //    if (EnitityId == "") {
    //        EnitityId = Enitity[i];
    //    }
    //    else {
    //        EnitityId += ',' + Enitity[i];
    //    }
    //}

    var Model = "";//gridModelLookup.gridView.GetSelectedKeysOnPage();
    var ModelId = "";
    //for (var i = 0; i < Model.length; i++) {
    //    if (ModelId == "") {
    //        ModelId = Model[i];
    //    }
    //    else {
    //        ModelId += ',' + Model[i];
    //    }
    //}

    // var ProbReport = gridProblemReportedLookup.gridView.GetSelectedKeysOnPage();
    var ProblemReportId = "";
    //for (var i = 0; i < ProbReport.length; i++) {
    //    if (ProblemReportId == "") {
    //        ProblemReportId = ProbReport[i];
    //    }
    //    else {
    //        ProblemReportId += ',' + ProbReport[i];
    //    }
    //}
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
                            url: "ServiceRegister.aspx/DeliveryReport",
                            data: JSON.stringify({ Data: Data }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                var status = "";
                                status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width:100%'>";
                                status = status + " <thead><tr>";
                                status = status + " <th>Receipt Challan</th><th>Date</th><th>Entry Type</th><th>Location</th><th>Entity Code</th><th>Network Name</th><th>Technician </th><th>Model</th>";
                                status = status + " <th>Serial No </th><th>Problem Reported </th><th>Service Action</th><th>Component(s)</th><th>Warranty</th>";
                                status = status + " <th>Warranty Status</th><th>Stock Entry</th><th>New Model</th><th>New Serial No</th><th>Billable</th><th>Return Reason</th>";
                                status = status + " <th>Problem Found</th><th>Remarks</th><th>Received By</th><th>Received On</th><th>Assign By</th><th>Assign On</th>";
                                status = status + " <th>Serviced By</th><th>Serviced On</th><th>Confirm Delivered</th><th>Delivered By</th><th>Delivered On</th><th>Status</th>";
                                status = status + " </tr></thead>";
                                status = status + " </table>";

                                $('#DivServiceRegisterReportDetails').html(status);

                                $('#dataTable').DataTable({
                                    scrollX: true,
                                    fixedColumns: {
                                        rightColumns: 1
                                    },
                                    data: msg.d,
                                    columns: [
                                       { 'data': 'ReceiptChallan' },
                                       { 'data': 'Date' },
                                       { 'data': 'EntryType' },
                                       { 'data': 'Location' },
                                       { 'data': 'EntityCode' },
                                       { 'data': 'NetworkName' },
                                       { 'data': 'Technician' },
                                       { 'data': 'Model' },
                                       { 'data': 'SerialNo' },
                                       { 'data': 'ProblemReported' },
                                       { 'data': 'ServiceAction' },
                                       { 'data': 'Component' },
                                       { 'data': 'Warranty' },
                                       { 'data': 'WarrantyStatus' },
                                       { 'data': 'StockEntry' },
                                       { 'data': 'NewModel' },
                                       { 'data': 'NewSerialNo' },
                                       { 'data': 'Billable' },
                                       { 'data': 'ReturnReason' },
                                       { 'data': 'ProblemFound' },
                                       { 'data': 'Remarks' },
                                       { 'data': 'ReceivedBy' },
                                       { 'data': 'ReceivedOn' },
                                       { 'data': 'AssignedBy' },
                                       { 'data': 'AssignedOn' },
                                       { 'data': 'ServicedBy' },
                                       { 'data': 'ServicedOn' },
                                       { 'data': 'ConfirmDelivery' },
                                       { 'data': 'DeliveredBy' },
                                       { 'data': 'DeliveredOn' },
                                       { 'data': 'Status' },
                                    ],
                                    dom: 'Bfrtip',
                                    buttons: [
                                        {
                                            extend: 'excel',
                                            title: null,
                                            filename: 'Service Register',
                                            text: 'Save as Excel',
                                            customize: function (xlsx) {
                                                var sheet = xlsx.xl.worksheets['sheet1.xml'];
                                                $('row:first c', sheet).attr('s', '42');
                                            },

                                            exportOptions: {
                                                // columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27]
                                            }
                                        }


                                    ],
                                    error: function (error) {
                                        alert(error);
                                    }
                                });
                                LoadingPanel.Hide();
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

//function selectAllProblemReported() {
//    gridProblemReportedLookup.gridView.SelectRows();
//}
//function unselectAllProblemReported() {
//    gridProblemReportedLookup.gridView.UnselectRows();
//}
//function CloseProblemReportedLookup() {
//    gridProblemReportedLookup.ConfirmCurrentSelection();
//    gridProblemReportedLookup.HideDropDown();
//    gridProblemReportedLookup.Focus();
//}
