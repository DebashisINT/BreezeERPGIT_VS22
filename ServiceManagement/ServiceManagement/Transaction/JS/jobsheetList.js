
        $(document).ready(function () {
            $('#dataTable').DataTable({
                scrollX: true,
                ordering: false,
                sortable: false,

                fixedColumns: {
                    rightColumns: 1
                }
            });


            //  $('#filterToggle').hide();

            $('.togglerSlide').click(function () {
                $('#filterToggle').slideDown({
                    duration: 200,
                    easing: "easeOutQuad"
                });
                $(this).hide()
            });
            $('.togglerSlidecut').click(function () {
                $('#filterToggle').slideUp({
                    duration: 200,
                    easing: "easeOutQuad"
                });
                $('.togglerSlide').show();
            });

            //$('[data-toggle="tooltip"]').tooltip();
            $(".date").datepicker({
                autoclose: true,
                todayHighlight: true,
                format: 'dd-mm-yyyy'
            }).datepicker('update', new Date());
        });

    //<%-- Code Added By Debashis Talukder For Document Printing End--%>
    var JOBId = 0;
function onPrintJv(id) {
    JOBId = id;
    cDocumentsPopup.Show();
    cCmbDesignName.SetSelectedIndex(0);
    cSelectPanel.PerformCallback('Bindalldesignes');
    $('#btnOK').focus();
}

function PerformCallToGridBind() {
    cSelectPanel.PerformCallback('Bindsingledesign');
    cDocumentsPopup.Hide();
    return false;
}

function cSelectPanelEndCall(s, e) {
    if (cSelectPanel.cpSuccess != null) {
        var TotDocument = cSelectPanel.cpSuccess.split(',');
        var reportName = cCmbDesignName.GetValue();
        var module = 'SRVJOBSHEET';
        window.open("../../../OMS/Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + JOBId, '_blank')
    }
    cSelectPanel.cpSuccess = null
    if (cSelectPanel.cpSuccess == null) {
        cCmbDesignName.SetSelectedIndex(0);
    }
}
//<%-- Code Added By Debashis Talukder For Document Printing End--%>

function JobSheetBtnClick() {
    var url = "jobsheetEntry.aspx?Key=ADD";
    window.location.href = url;
}

function Edit(values) {
    location.href = "jobsheetEntry.aspx?key=Edit&id=" + values;
}

function View(values) {
    location.href = "jobsheetEntry.aspx?key=View&id=" + values;
}

function SearchClick() {
    LoadingPanel.Show();
    var brnch ="";// gridbranchLookup.gridView.GetSelectedKeysOnPage();
    var branchss = "";
    //for (var i = 0; i < brnch.length; i++) {
    //    branchss += ',' + brnch[i];
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

    var fromdtSplit = $("#FromDate").val().split('-');
    var fromdt = fromdtSplit[2] + '-' + fromdtSplit[1] + '-' + fromdtSplit[0];

    var todtsplit = $("#ToDate").val().split('-');
    var todt = todtsplit[2] + '-' + todtsplit[1] + '-' + todtsplit[0];

    var SearchType = "";

    gridbranchLookup.gridView.GetSelectedFieldValues("ID", function (val) {
        brnch = val

        for (var i = 0; i < brnch.length; i++) {
            if (branchss == "") {
                branchss = brnch[i];
            }
            else {
                branchss += ',' + brnch[i];
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
            var Apply = {
                FromDate: fromdt,
                ToDate: todt,
                Branch: branchss,//$("#ddlBranch").val(),
                SearchType: "",
                Technician_ID: Technician//$("#ddlTechnician").val()
            }


            $.ajax({
                type: "POST",
                url: "jobsheetList.aspx/JobSheetList",
                data: JSON.stringify({ model: Apply }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    var status = "";
                    status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width:100%'>";
                    status = status + " <thead><tr>";
                    status = status + " <th>Challan Number</th><th>Posting Date</th><th>Ref Jobsheet</th><th>Assign To </th><th>Work done on</th>";
                    status = status + " <th>Location</th><th>Entity Code</th><th>Network Name </th><th>Contact Person </th>";
                    status = status + " <th>Contact Number</th><th>Entered By</th><th>Entered On</th><th>Status</th><th>Action</th>";
                    status = status + " </tr></thead>";
                    status = status + " </table>";

                    $('#divListData').html(status);

                    $('#dataTable').DataTable({
                        ordering: false,
                        sortable: false,
                        scrollX: true,

                        fixedColumns: {
                            rightColumns: 2
                        },
                        data: msg.d,
                        columns: [
                           { 'data': 'ChallanNumber' },
                           { 'data': 'PostingDate' },
                           { 'data': 'RefJobsheet' },
                           { 'data': 'AssignTo' },
                           { 'data': 'WorkDoneOn' },
                           { 'data': 'Location' },
                           { 'data': 'EntityCode' },
                           { 'data': 'NetworkName' },
                           { 'data': 'ContactPerson' },
                           { 'data': 'ContactNumber' },
                           { 'data': 'Receivedby' },
                           { 'data': 'Receivedon' },
                           { 'data': 'Status' },
                           { 'data': 'Action' },
                        ],
                        dom: 'Bfrtip',
                        buttons: [
                            //'copy', 'csv', 'excel', 'pdf', 'print'
                             {
                                 extend: 'excel',
                                 title: null,
                                 filename: 'JobSheet',
                                 text: 'Save as Excel',
                                 customize: function (xlsx) {
                                     var sheet = xlsx.xl.worksheets['sheet1.xml'];
                                     $('row:first c', sheet).attr('s', '42');
                                 },

                                 exportOptions: {
                                     columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
                                 }
                             }
                        ],
                        error: function (error) {
                            alert(error);
                            LoadingPanel.Hide();
                        }
                    });
                    LoadingPanel.Hide();
                }
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

function Delete(values) {

    jConfirm('Confirm Delete?', 'Alert', function (r) {
        if (r) {
            $.ajax({
                type: "POST",
                url: "jobsheetList.aspx/DeleteJobsheetEntry",
                data: JSON.stringify({ JobsheetID: values }),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    console.log(response);
                    if (response.d) {
                        if (response.d.split('~')[1] == "Success") {
                            jAlert(response.d.split('~')[0], "Alert", function () {
                                SearchClick();
                            });
                        }
                        else {
                            jAlert(response.d.split('~')[0]);
                            return
                        }
                    }
                },
                error: function (response) {
                    console.log(response);
                }
            });
        }
    });
}

$(function () {
    cBranchPanel.PerformCallback('BindComponentGrid' + '~' + "All");
    cTechnicianPanel.PerformCallback('BindTechnicianGrid' + '~' + "All");
});

function selectAllBranch() {
    gridbranchLookup.gridView.SelectRows();
}
function unselectAllBranch() {
    gridbranchLookup.gridView.UnselectRows();
}
function CloseBranchLookup() {
    gridbranchLookup.ConfirmCurrentSelection();
    gridbranchLookup.HideDropDown();
    gridbranchLookup.Focus();
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
