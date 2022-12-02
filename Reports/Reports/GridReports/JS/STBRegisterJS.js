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
    //cModelPanel.PerformCallback('BindModelGrid' + '~' + "All");
    //cProblemFoundPanel.PerformCallback('BindProblemFoundGrid' + '~' + "All");
    //cTechnicianPanel.PerformCallback('BindTechnicianGrid' + '~' + "All");
    cLocationPanel.PerformCallback('BindLocationGrid' + '~' + "All");
    //  cProblemReportedPanel.SetEnabled(false);
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

function ddlType_change() {

    var Report = '';
    Report = Report + ' <select id="ddlReport" class="multi" multiple="multiple" > ';
    if ($("#ddlType").val() != null) {
        if ($("#ddlType").val().includes("Money Receipt")) {
            Report = Report + '  <option value="On Account">On Account</option>';
            //  document.getElementById("customCheck1").disabled = false;
        }
        if ($("#ddlType").val().includes("Wallet Recharge")) {
            Report = Report + '  <option value="Wallet Recharge">Wallet Recharge</option>';
            //document.getElementById("customCheck1").disabled = true;
        }
        if ($("#ddlType").val().includes("STB Requisition")) {
            Report = Report + '  <option value="1">Fresh Issue</option>';
            Report = Report + '  <option value="2">Replacement</option>';
            Report = Report + '  <option value="3">OSTB Exchange</option>';
            Report = Report + '  <option value="4">STB Upgradation</option>';
            Report = Report + '  <option value="9">Scheme</option>';
            // document.getElementById("customCheck1").disabled = true;
        }
        if ($("#ddlType").val().includes("Return Requisition")) {
            Report = Report + '  <option value="7">STB-01</option>';
            Report = Report + '  <option value="8">STB-02</option>';
            // document.getElementById("customCheck1").disabled = false;
        }
    }

    Report = Report + ' </select>';

    $('#divReportType').html(Report);
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

    $("#hdnType").val($("#ddlType").val());
    $("#hdnReportType").val($("#ddlReport").val());
    $("#hdnFromDate").val($("#dtFromdate").val());
    $("#hdnToDate").val($("#dtToDate").val());
    $("#hdnIsDetails").val(IsDetailsReport);



    if ($("#ddlType").val() != null) {
        if ($("#ddlReport").val() != null) {
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
        else {
            jAlert("Please select Report Type.");
            $("#ddlReport").focus();
        }
    }
    else {
        jAlert("Please select Type.");
        $("#ddlType").focus();
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

