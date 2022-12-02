
$(document).ready(function () {



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

    //$('#dataTable').DataTable({
    //scrollX: true,
    //ordering: false,
    //sortable: false,
    //fixedColumns: {
    //    rightColumns: 2
    //}
    //});

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
    var url = "stbAdd.aspx?Key=ADD";
    window.location.href = url;
}

function Edit(values) {
    location.href = "stbAdd.aspx?key=Edit&id=" + values;
}

function View(values) {
    location.href = "stbAdd.aspx?key=View&id=" + values;
}

function SearchClick() {
    LoadingPanel.Show();
    var brnch = "";
    var branchss = "";

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

        var Apply = {
            FromDate: fromdt,
            ToDate: todt,
            Branch: branchss,
            SearchType: ""
        }

        $.ajax({
            type: "POST",
            url: "stbList.aspx/STBReceivedList",
            data: JSON.stringify({ model: Apply }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                $('#dataTable').DataTable().destroy();
                var status = "";
                status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width:100%'>";
                status = status + " <thead><tr>";
                status = status + " <th>Document No</th><th>Date</th><th>Unit </th>";//<th>Location</th>
                status = status + " <th>Remarks </th><th>Entered by</th><th>Entered On</th>";
                status = status + " <th>Action</th>";
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
                       { 'data': 'DocumentNo' },
                       { 'data': 'Date' },
                       { 'data': 'Unit' },
                       //{ 'data': 'Location' },
                       { 'data': 'Remarks' },
                       { 'data': 'Receivedby' },
                       { 'data': 'Receivedon' },
                       { 'data': 'Action' },
                    ],
                    dom: 'Bfrtip',
                    buttons: [
                        //'copy', 'csv', 'excel', 'pdf', 'print'
                         {
                             extend: 'excel',
                             title: null,
                             filename: 'STBReceived',
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
                url: "stbList.aspx/DeleteSTB",
                data: JSON.stringify({ STBReceivedID: values }),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    console.log(response);
                    if (response.d) {
                        if (response.d.split('~')[1] == "Success") {
                            jAlert(response.d.split('~')[0], "Alert", function () {
                                cGrdSTBListing.Refresh();
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
   // cBranchPanel.PerformCallback('BindComponentGrid' + '~' + "All");
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


function updateGridByDate() {
    //debugger;
    if (cFormDate.GetDate() == null) {
        jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
    }
    else if (ctoDate.GetDate() == null) {
        jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
    }
    else if (ccmbBranchfilter.GetValue() == null) {
        jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
    }
    else {
        localStorage.setItem("FromDateSalesOrder", cFormDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("ToDateSalesOrder", ctoDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("OrderBranch", ccmbBranchfilter.GetValue());
        //cGrdOrder.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());
        $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilter.GetValue());
        $("#hfIsFilter").val("Y");
        cGrdSTBListing.Refresh();
    }
}

function gridRowclick(s, e) {
    //alert('hi');
    $('#GrdSTBListing').find('tr').removeClass('rowActive');
    $('.floatedBtnArea').removeClass('insideGrid');
    $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
    $(s.GetRow(e.visibleIndex)).addClass('rowActive');
    setTimeout(function () {
        var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
        $.each(lists, function (index, value) {
            setTimeout(function () {
                $(value).css({ 'opacity': '1' });
            }, 100);
        });
    }, 200);
}