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
    cLocationPanel.PerformCallback('BindLocationGrid' + '~' + "All");
});


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


function Callback_EndCallback() {
    $("#drdExport").val(0);
}


function Generate_Report() {

    var IsDetailsReport = "0";
    if ($('#customCheck1').is(':checked') == true) {
        IsDetailsReport = "1";

        $("#DivSTBRegisterReportSummary").addClass("hide");
        $("#DivSTBRegisterReportDetails").removeClass("hide");
        $("#hdnGridTytpe").val("Details");
    }
    else {
        $("#DivSTBRegisterReportSummary").removeClass("hide");
        $("#DivSTBRegisterReportDetails").addClass("hide");
        $("#hdnGridTytpe").val("Summary");
    }

    $("#hdnFromDate").val($("#dtFromdate").val());
    $("#hdnToDate").val($("#dtToDate").val());
    $("#hdnIsDetails").val(IsDetailsReport);

    if ($("#dtFromdate").val() != "") {
        if ($("#dtToDate").val() != "") {
            var data = "OnDateChanged";
            $("#hfIsSTBMatIORegDetFilter").val("Y");
            cCallbackPanel.PerformCallback(data);
        }
        else {
            jAlert("Please select To Date.");
            $("#dtTodate").focus();
        }
    }
    else {
        jAlert("Please select From Date.");
        $("#dtFromDate").focus();
    }
}


function CallbackPanelEndCall(s, e) {
    if ($('#customCheck1').is(':checked') == true) {
        $("#DivSTBRegisterReportSummary").addClass("hide");
        $("#DivSTBRegisterReportDetails").removeClass("hide");
        Grid.Refresh();
    }
    else {
        $("#DivSTBRegisterReportSummary").removeClass("hide");
        $("#DivSTBRegisterReportDetails").addClass("hide");
        GridHeader.Refresh();
    }
}
