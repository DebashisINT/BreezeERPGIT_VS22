
    function isValidEmailAddress(emailAddress) {
        var pattern = new RegExp(/^(("[\w-\s]+")|([\w-]+(?:\.[\w-]+)*)|("[\w-\s]+")([\w-]+(?:\.[\w-]+)*))(@@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@@\[?((25[0-5]\.|2[0-4][0-9]\.|1[0-9]{2}\.|[0-9]{1,2}\.))((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){2}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\]?$)/i);
        return pattern.test(emailAddress);
    }


    $(document).ready(function () {
        $("#hfActivityFilter").val("Y");
        $('#dataTbl').DataTable({
            "searching": false,
            "bInfo": false,
            "info": false,
            "ordering": false,
            "paging": false
        });
        $('#dataTblCRM').DataTable({
            "searching": false,
            "bInfo": false,
            "info": false,
            "ordering": false,
            "paging": false
        });

        //$("#Expected_Response").mask("#0.00", { reverse: true });

    })


$(function () {
    $('#ddlExport').on('change', function () {
        if ($("#ddlExport option:selected").index() > 0) {
            var selectedValue = $(this).val();
            $('#ddlExport').prop("selectedIndex", 0);

            var url = $('#hdnExportLink').val();
            window.location.href = url.replace("_type_", selectedValue);
        }
    });
});





function CalculateTotalCost() {
    var tot = cActivity_Cost.GetValue() - cMisc_Cost.GetValue();
    cTotal_Cost.SetValue(tot);
}




function OnAddbuttonClick() {
    $("#hdnCampaign_Id").val("");
    $("#hdnCrmProductIdentityId").val("");
    $("#hdnCampaign_Id").val("");
    $("#hdnModule_Name").val("");
    $("#hdnModule_Id").val("");
    $("#hdnCampaign_Id").val("");
    $("#hdnCrmProductIdentityId").val("");

    $("#projectMod").modal('show');
    cCampaign_Name.SetEnabled(true);
    cCampaign_Code.SetEnabled(true);

    cCampaign_Name.SetText("");
    cCampaign_Code.SetText("");
    $("#TYPE_ID").val(0);
    cExpected_Response.SetValue(0);
    if (response.Expected_Start != null)
        cExpected_Start.SetDate(new Date());
    if (response.Expected_End != null)
        cExpected_End.SetDate(new Date());
    if (response.Actual_Start != null)
        cActual_Start.SetDate(new Date());
    if (response.Actual_End != null)
        cActual_End.SetDate(new Date());

    cActivity_Cost.SetValue(0);
    cAllocated_Budget.SetValue(0);
    cMisc_Cost.SetValue(0);
    cTotal_Cost.SetValue(0);
    cOffer.SetText("");
    cEstimate_Revenue.SetText(0);
    // cWoner.SetText("");
    $("#Status_Id").val("");
}

function Expected_Responsechange() {
    $("#Expected_Response").validate();
}


function EditClick(id) {
    $("#hdnCampaign_Id").val(id);
    var obj = {};
    obj.Campaign_Id = id;
    LoadingPanel.Show();
    $.ajax({
        type: "POST",
        url: "/Campaign/EditCampaign",
        data: JSON.stringify(obj),
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            LoadingPanel.Hide();
            cCampaign_Name.SetText(response.Campaign_Name);
            cCampaign_Code.SetText(response.Campaign_Code);
            cCampaign_Name.SetEnabled(false);
            cCampaign_Code.SetEnabled(false);

            $("#TYPE_ID").val(response.TYPE_ID);
            cExpected_Response.SetValue(parseFloat(response.Expected_Response));
            if (response.Expected_Start != null)
                cExpected_Start.SetDate(new Date(parseInt(response.Expected_Start.substr(6))));
            if (response.Expected_End != null)
                cExpected_End.SetDate(new Date(parseInt(response.Expected_End.substr(6))));
            if (response.Actual_Start != null)
                cActual_Start.SetDate(new Date(parseInt(response.Actual_Start.substr(6))));
            if (response.Actual_End != null)
                cActual_End.SetDate(new Date(parseInt(response.Actual_End.substr(6))));

            cActivity_Cost.SetValue(parseFloat(response.Activity_Cost));
            cAllocated_Budget.SetValue(parseFloat(response.Allocated_Budget));
            cMisc_Cost.SetValue(parseFloat(response.Misc_Cost));
            cTotal_Cost.SetValue(parseFloat(response.Total_Cost));
            cOffer.SetText(response.Offer);
            cEstimate_Revenue.SetValue(parseFloat(response.Estimate_Revenue));
            //cWoner.SetText(response.Woner);

            $("#WonerID").val(response.WonerID);
            $("#AssignedID").val(response.AssignedID);


            $("#Status_Id").val(response.Status_Id);
            $("#SourceID").val(response.SourceID);
            $("#projectMod").modal('show');
        },
        error: function (response) {
            jAlert("Can not Edit");
            LoadingPanel.Hide();
        }
    });
}
function DeleteClick(id) {
    $("#hdnCampaign_Id").val(id);

    var obj = {};
    obj.Campaign_Id = id;
    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
        if (r == true) {
            $.ajax({
                type: "POST",
                url: "/Campaign/DeleteCampaign",
                data: JSON.stringify(obj),
                async: true,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    jAlert(response);
                    gridcrmCampaign.Refresh();
                    gridcrmCampaign.Refresh();
                },
                error: function (response) {
                    jAlert("Can not Delete");
                    gridcrmCampaign.Refresh();
                    gridcrmCampaign.Refresh();
                    LoadingPanel.Hide();
                }
            });
        }
    });
}
function DoActivity(id) {
    $("#hdnCampaign_Id").val(id);
    $.ajax({
        type: "POST",
        //url: "Url.Action("GetEmployeesTargetByCode", "EmployeesTarget")",
        url: "@Url.Action("DoActivity", "CRMActivity")",
        data: { Module_Name: "Campaign Activity", Module_id: id },
    success: function (response) {
        CRMpcControl.SetContentHtml(response);
        MVCxClientUtils.FinalizeCallback();
        CRMpcControl.SetHeaderText('Activities');
        CRMpcControl.Show();


    }
});
}

function CancelActivity() {
    CRMpcControl.Hide();
}
function DoSharing(id) {
    $("#hdnCampaign_Id").val(id);
    $("#hdnModule_Name").val("Campaign");
    $("#hdnModule_Id").val(id);
    $("#toinput").tagsinput('add', "", { preventPost: true });
    $("#PhoneInput").tagsinput('add', "", { preventPost: true });
    $.ajax({
        type: "POST",
        //url: "Url.Action("GetEmployeesTargetByCode", "EmployeesTarget")",
        url: "@Url.Action("GetEntityDetails", "CRMSharing")",
        data: { Module_Name: "Campaign", Module_id: id },
    async: false,
    success: function (response) {


        var emails = response.emails.map(function (elem) {
            return elem.Entity_Email;
        }).join(",");

        $("#toinput").tagsinput('add', emails, { preventPost: true });

        var sms = response.phones.map(function (elem) {
            return elem.Entity_Phone;
        }).join(",");

        $("#PhoneInput").tagsinput('add', sms, { preventPost: true });

    }

});
$("#sharingmodel").modal('show');

}
function DoProducts(id) {
    $("#hdnCampaign_Id").val(id);
    $("#hdnCrmProductIdentityId").val(id);
    ShowProducts('ACPRD');


    ShowCRMProductsEditCRM('Campaign Products', id);

}
function AddMember(id) {
    $("#Module_Name").val('Campaign');
    $("#hdnCampaign_Id").val(id);

    $.ajax({
        type: "POST",
        //url: "Url.Action("GetEmployeesTargetByCode", "EmployeesTarget")",
        url: "@Url.Action("doAddMembers", "CRMMembers")",
        data: { Module_Name: "Campaign", Module_id: id },
    success: function (response) {
        CRMpcControl.SetContentHtml(response);
        MVCxClientUtils.FinalizeCallback();
        CRMpcControl.SetHeaderText('Members');

        CRMpcControl.Show();


    }
});

}
function AddLiterature(id, name, code) {


    $('#AttachmentModal').modal('show');

    $("#hdnCampaign_Id").val(id);
    $('#hdnDocNo').val(id);
    $('#hdndoc_id').val(id);
    //////Do Not need in Others module
    $("#docFileName").val(code);
    $("#docNumber").val(name);
    document.getElementById("docFileName").disabled = true;
    document.getElementById("docNumber").disabled = true;
    ////
    setTimeout(function () { $('#documentType').focus(); }, 1000);
}


function SaveCampaign() {

    if (cCampaign_Name.GetText() == "") {
        jAlert('Please enter a valid name to procced.', 'Alert');
        return;
    }

    var obj = {};
    if ($("#hdnCampaign_Id").val() == "") {
        obj.Action = "Add";
    }
    else {
        obj.Action = "Update";
    }

    obj.Campaign_Id = $("#hdnCampaign_Id").val();
    obj.Campaign_Name = cCampaign_Name.GetText();
    obj.Campaign_Code = cCampaign_Code.GetText();
    obj.TYPE_ID = $("#TYPE_ID").val();
    obj.Expected_Response = cExpected_Response.GetText();
    obj.Expected_Start = cExpected_Start.GetDate();
    obj.Expected_End = cExpected_End.GetDate();
    obj.Actual_Start = cActual_Start.GetDate();
    obj.Actual_End = cActual_End.GetDate();
    obj.Activity_Cost = cActivity_Cost.GetText();
    obj.Allocated_Budget = cAllocated_Budget.GetText();
    obj.Misc_Cost = cMisc_Cost.GetText();
    obj.Total_Cost = cTotal_Cost.GetText();
    obj.Offer = cOffer.GetText();
    obj.Estimate_Revenue = cEstimate_Revenue.GetText();
    //obj.Woner = cWoner.GetText();
    obj.WonerId = $("#WonerID").val();
    obj.AssignedId = $("#AssignedID").val();
    obj.Status_Id = $("#Status_Id").val();
    obj.SourceID = $("#SourceID").val();

    LoadingPanel.Show();
    $.ajax({
        type: "POST",
        url: "/Campaign/SaveCampaign",
        data: JSON.stringify(obj),
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            console.log(response);
            jAlert(response);
            gridcrmCampaign.Refresh();
            gridcrmCampaign.Refresh();
            $("#projectMod").modal('hide');
            LoadingPanel.Hide();
        },
        error: function (response) {
            jAlert("Please try again later");
            LoadingPanel.Hide();
        }
    });

}
function ActivityChange(s, e) {
    $.ajax({
        type: "POST",
        //url: "Url.Action("GetEmployeesTargetByCode", "EmployeesTarget")",
        url: "@Url.Action("ActivityChange", "CRMActivity")",
        data: { ActivityId: s.GetValue() },
    success: function (response) {
        cddlActivityType.ClearItems();
        cddlActivityType.AddItem("Select", "0");
        for (var i = 0; i < response.length; i++) {
            cddlActivityType.AddItem(response[i].Lead_ActivityTypeName, response[i].Id);
        }
        cddlActivityType.SetSelectedIndex(0);
    }

});

}
$(document).ready(function () {
    $("body").bind("keydown", function (event) {
        if (event.keyCode == 65 && event.altKey == true) {
            OnAddbuttonClick();
        }
    });
});



$(function () {
    $("#hdnCampaign_Id").val("")
    //gridcrmCampaign.Refresh();
    //gridcrmCampaign.Refresh();
})
$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})
function gridcrmCampaignclick(s, e) {
    $('#gridcrmCampaign').find('tr').removeClass('rowActive');
    $('.floatedBtnArea').removeClass('insideGrid');
    //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
    $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
    $(s.GetRow(e.visibleIndex)).addClass('rowActive');
    setTimeout(function () {
        //alert('delay');
        var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
        //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
        //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
        //    setTimeout(function () {
        //        $(this).fadeIn();
        //    }, 100);
        //});
        $.each(lists, function (index, value) {
            //console.log(index);
            //console.log(value);
            setTimeout(function () {
                console.log(value);
                $(value).css({ 'opacity': '1' });
            }, 100);
        });
    }, 200);
}
function OnAddActivitiesbuttonClick() {
    cActivity_Date.SetDate(new Date());
    cddlActivity.SetValue("");
    cddlActivityType.SetValue("");
    ctxt_Subject.SetText("");
    cmemo_Details.SetText("");
    $("#ddlPriority").val(0);
    cDue_dt.SetDate(new Date());
    $("#btnClear").addClass('hide');
    $("#btnSave").removeClass('hide');
}

function ShowAllClick() {
    gridcrmCampaign.Refresh();
    gridcrmCampaign.Refresh();

}

