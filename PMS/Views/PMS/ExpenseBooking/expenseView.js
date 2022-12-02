
    function PopulateProjectData() {
        $.ajax({
            type: "post",
            url: "@Url.Action("GetProjectCode", "ExpenseBooking")",
            data: { BranchId: $('#ddlBranch').val() },
        datatype: "json",
        traditional: true,
        success: function (data) {

            var status = "<select id='ddlProjects'>";
            status = status + '<option value="0">Select</option>';
            for (var i = 0; i < data.length; i++) {
                status = status + '<option value=' + data[i].Proj_Id + '>' + data[i].Proj_Name + '</option>';
            }
            status = status + '</select>';
            $('#Projectdiv').html(status);
        }
    });
}

function validateclass() {
    return true;
}

function SaveExit() {
    if (validateclass()) {
        var action = "ADD";
        if ($('#hdnAction').val() == "Edit") {
            action = "EDIT";
        }
        else {
            action = "ADD";
            $('#hdnuniqueid').val("");

        }

        var txtaftertimelate = "";
        var txtaftertimelatehh = $('#txtaftertimelatehh').val();
        var txtaftertimelatemm = $('#txtaftertimelatemm').val();
        if (txtaftertimelatemm == "") {
            txtaftertimelatemm = "00";
        }
        if (txtaftertimelatehh == "") {
            txtaftertimelatehh = "00";
        }
        if (txtaftertimelatehh != "") {
            txtaftertimelate = (txtaftertimelatehh + ":" + txtaftertimelatemm);//$('#txtConsAttenAfter').val();
        }
        var StrDate = StartDate_dt.GetDate();

        var BranchID = document.getElementById("ddlBranch");
        var strBranchID = BranchID.options[BranchID.selectedIndex].value;

        var e = document.getElementById("ddlProjects");
        var strProjects = e.options[e.selectedIndex].value;

        var Expense_Category = document.getElementById("ddlExpenseCategory");
        var strExpenseCategory = Expense_Category.options[Expense_Category.selectedIndex].value;

        var ddlBasis = document.getElementById("ddlBasis");
        var strBasis = ddlBasis.options[ddlBasis.selectedIndex].value;

        var ddlCurrency = document.getElementById("ddlCurrency");
        var strCurrency = ddlCurrency.options[ddlCurrency.selectedIndex].value;

        if (StrDate == null) {
            jAlert("Select Transaction Date.");
            return false;
        }

        if (strBranchID == "0") {
            jAlert("Select Unit.");
            return
        }

        if (strProjects == "0") {
            jAlert("Select Projects.");
            return
        }

        if (strExpenseCategory == "0") {
            jAlert("Select Expense Category.");
            return
        }

        if (strBasis == "") {
            jAlert("Select Basis.");
            return
        }

        if (cQuantity.GetValue() <= 0) {
            jAlert("Enter Quantity.");
            return
        }

        if (cPrice.GetValue() <= 0) {
            jAlert("Enter Price.");
            return
        }

        if (strCurrency == "0") {
            jAlert("Select Currency.");
            return
        }

        var obj = {};
        obj.ExpensePurpose = $("#txtExpensePurpose").val();

        obj.TransactionDate = StrDate;
        obj.Quantity = cQuantity.GetValue();
        obj.Price = cPrice.GetValue();
        obj.Amount = cAmount.GetValue();
        obj.SalesTaxAmount = cSalesTaxAmount.GetValue();
        obj.ExternalComments = $("#txtExternalComments").val();
        obj.Projects = strProjects;
        obj.Basis = strBasis;
        obj.ExpenseCategory = strExpenseCategory;
        obj.Currency = strCurrency;
        obj.Action_type = action
        obj.ExpenseBooking_Id = $('#hdnExpenseBookingid').val();
        obj.BranchID = strBranchID;

        $.ajax({
            type: "POST",
            url: "@Url.Action("SaveData", "ExpenseBooking")",
            contentType: "application/json; charset=utf-8",
        dataType: "json",
        //data: { timeSt: obj, uniqueid: $('#hdnuniqueid').val(), Date: varformat_startdt },
        data: JSON.stringify(obj),
        success: function (response) {
            if (response.response_msg == "Success") {
                jAlert("Saved Successfully", "Alert", function () {
                    var url = '/ExpenseBooking/ExpenseBookingView';
                    window.location.href = url;
                });
            }
            else if (response.response_msg == "Update") {
                jAlert("Update Successfully", "Alert", function () {
                    $('#ExpBooking').modal('toggle');
                    is_pageload = "1";
                    isshowclicked = "1";
                    //gridExpenseBookingList.Refresh();
                    Show();
                });
            }
            else {
                jAlert(response.response_msg);
            }
        },
        error: function (response) {
            jAlert("Please try again later", "Alert", function () {
            });
        }
    });
}
}

function OnStartCallback(s, e) {

}

function gridRowclick(s, e) {
    $('#gridExpenseBookingList').find('tr').removeClass('rowActive');
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

    function OpenExpenseBookingforEdit(obj) {
        //alert(obj);
        $("#hdnExpenseBookingid").val(obj);
        $.ajax({
            type: "POST",
            url: "@Url.Action("ViewDataset", "ExpenseBooking")",
            data: { ExpenseBooking_id: obj },
        dataType: "json",
        success: function (response) {
            var status = response;
            var str = "";

            if (status != null) {
                $("#exampleModalLabel").html("Edit Expense");
                StartDate_dt.SetDate(new Date(parseInt(status.Transaction_Date.substr(6))));
                $("#ddlBranch").val(status.BranchID);
                PopulateProjectData();
                $("#txtExpensePurpose").val(status.ExpensePurpose);
                $("#ddlExpenseCategory").val(status.ExpenseCategory);
                $("#ddlBasis").val(status.Basis);
                cQuantity.SetValue(status.Quantity);
                cPrice.SetValue(status.Price);
                cAmount.SetValue(status.Amount);
                cSalesTaxAmount.SetValue(status.SalesTaxAmount);
                //$("#txtQuantity").val(status.Quantity);
                //$("#txtPrice").val(status.Price);
                $("#ddlCurrency").val(status.Currency);
                //$("#txtAmount").val(status.Amount);
                //$("#txtSalesTaxAmount").val(status.SalesTaxAmount);
                $("#txtExternalComments").val(status.ExternalComments);
                setTimeout(function () {
                    $("#ddlProjects").val(status.Projects);
                }, 200);
                $("#hdnAction").val('Edit');
                $("#ExpBooking").modal('toggle');
                $("#btnSave").removeClass('hide');
            }
        },
        error: function (response) {
            //   alert(response);
            jAlert("Please try again later.");
            //LoadingPanel.Hide();
        }
    });
}

function OpenExpenseBookingDelete(obj) {
    jConfirm('Confirm Delete?', 'Alert', function (r) {
        if (r) {
            // $("#hdnExpenseBookingid").val(obj);
            $.ajax({
                type: "POST",
                url: "@Url.Action("DeleteData", "ExpenseBooking")",
                data: { ExpenseBooking_id: obj },
            dataType: "json",
            success: function (response) {
                var status = response;
                jAlert(status);
                gridExpenseBookingList.Refresh();
            },
            error: function (response) {
                //  alert(response);
                jAlert("Please try again later.");
            }
        });
    }
    else {
    // alert("false");
}
});
}



function OpenExpenseBookingforView(obj) {
    //alert(obj);
    $("#hdnExpenseBookingid").val(obj);
    $.ajax({
        type: "POST",
        url: "@Url.Action("ViewDataset", "ExpenseBooking")",
        data: { ExpenseBooking_id: obj },
    dataType: "json",
    success: function (response) {
        var status = response;
        var str = "";

        if (status != null) {
            $("#exampleModalLabel").html("View Expense");
            // $("#SkillName").prop('disabled', true);
            StartDate_dt.SetDate(new Date(parseInt(status.Transaction_Date.substr(6))));
            $("#ddlBranch").val(status.BranchID);
            PopulateProjectData();

            $("#txtExpensePurpose").val(status.ExpensePurpose);
            $("#ddlExpenseCategory").val(status.ExpenseCategory);
            $("#ddlBasis").val(status.Basis);
            //$("#txtQuantity").val(status.Quantity);
            cQuantity.SetValue(status.Quantity);
            cPrice.SetValue(status.Price);
            cAmount.SetValue(status.Amount);
            cSalesTaxAmount.SetValue(status.SalesTaxAmount);
            //$("#txtPrice").val(status.Price);
            $("#ddlCurrency").val(status.Currency);
            //$("#txtAmount").val(status.Amount);
            //$("#txtSalesTaxAmount").val(status.SalesTaxAmount);
            $("#txtExternalComments").val(status.ExternalComments);
            setTimeout(function () {
                $("#ddlProjects").val(status.Projects);
            }, 200);
            $("#hdnAction").val('View');
            $("#ExpBooking").modal('toggle');
            $("#btnSave").addClass('hide');
        }
    },
    error: function (response) {
        jAlert("Please try again later.");
    }
});
}

function Show() {
    var frm_dt = FromDate.GetDate();
    var FromDt = frm_dt;
    var to_dt = ToDate.GetDate();
    var ToDt = to_dt;
    var Unit = $("#ddlunitlist").val();
    var BranchID = Unit;
    var model = {};

    model.BranchID = BranchID,
    model.FromDate = FromDt,
    model.ToDate = ToDt
    $.ajax({
        type: "POST",
        url: "@Url.Action("ShowExpenseBookingPartial", "ExpenseBooking")",
        // data: { BranchID: BranchID, FromDate: FromDt, ToDate: ToDt },
        contentType: "application/json; charset=utf-8",
    data: JSON.stringify(model),
    dataType: "json",
    //traditional: true,
    success: function (response) {
        var status = response;
        gridExpenseBookingList.Refresh();
    },
    error: function (response) {
        jAlert(response);
    }
});
}

function Clear() {
    $("#ddlBranch").val(0);
    $("#ddlProjects").val(0);
    $("#txtExpensePurpose").val('');
    $("#ddlExpenseCategory").val(0);
    $("#ddlBasis").val('');
    cQuantity.SetValue(0);
    //$("#txtPrice").val('0.00');
    cPrice.SetValue(0);
    $("#ddlCurrency").val(0);
    //$("#txtAmount").val('0.00');
    cAmount.SetValue(0);
    //$("#txtSalesTaxAmount").val('0.00');
    cSalesTaxAmount.SetValue(0);
    $("#txtExternalComments").val('');
    $("#hdnAction").val('');
    $("#btnSave").removeClass('hide');
}

function ExpenseSetTotalAmount(s, e) {
    var Qty = cQuantity.GetValue();
    var Price = cPrice.GetValue();
    var tAmount = 0;
    tAmount = Qty * Price;
    cAmount.SetValue(tAmount);
}

function cmbExport_SelectedIndexChanged() {
    var exportid = $('#exportlist option:selected').val();
    // $('#exportlist').val(0);
    if (exportid > 0) {
        var url = '@Url.Action("ExportExpenseBookingGridList", "ExpenseBooking", new { type = "_type_" })'
        window.location.href = url.replace("_type_", exportid);
    }
}


    $(function () {
        //BindProject();
        PopulateUnitList();
        if (localStorage.getItem('FromDateExpenseBookingEntry')) {
            FromDate.SetDate(new Date(localStorage.getItem('FromDateExpenseBookingEntry')));
        }
        if (localStorage.getItem('ToDateExpenseBookingEntry')) {
            ToDate.SetDate(new Date(localStorage.getItem('ToDateExpenseBookingEntry')));
        }
        $('#btnAddNew').focus();
    });

function DateValidateTo() {
    if (FromDate.GetDate()) {

        var StrDate = ToDate.GetDate();
        var varformat_startdt = (StrDate.getDate() - 60) + "-" + ((StrDate.getMonth() + 1) < 10 ? '0' + (StrDate.getMonth() + 1) : (StrDate.getMonth() + 1)) + "-" + StrDate.getFullYear();

        FromDate.SetText(varformat_startdt);

        if (ToDate.GetDate() < FromDate.GetDate()) {
            ToDate.SetValue(FromDate.GetDate());
        }
    }
}
function DateValidateFrom() {
    if (ToDate.GetDate()) {
        var StrDate = FromDate.GetDate();
        var varformat_startdt = (StrDate.getDate() + 60) + "-" + ((StrDate.getMonth() + 1) < 10 ? '0' + (StrDate.getMonth() + 1) : (StrDate.getMonth() + 1)) + "-" + StrDate.getFullYear();

        ToDate.SetText(varformat_startdt);
        if (ToDate.GetDate() < FromDate.GetDate()) {
            ToDate.SetValue(FromDate.GetDate());
        }
    }
}

function PopulateUnitList() {
    $.ajax({
        type: "POST",
        url: "@Url.Action("PopulateBranchByHierchy", "ExpenseBooking")",
        success: function (response) {
            var html = "";
            for (var i = 0; i < response.length; i++) {
                html = html + "<option value='" + response[i].ID + "'>" + response[i].Name + "</option>";
            }
            $('#ddlunitlist').html(html);

            if (localStorage.getItem('TimeSheetBranch')) {
                if ($("#ddlunitlist option[value=" + localStorage.getItem('TimeSheetBranch') + "]").text() != "" && $("#ddlunitlist option[value=" + localStorage.getItem('TimeSheetBranch') + "]").text() != null) {
                    $('#ddlunitlist').val(localStorage.getItem('TimeSheetBranch'));
                }
            }
        }
});
}
