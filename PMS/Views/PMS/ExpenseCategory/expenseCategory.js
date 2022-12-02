
    $(function () {
        gridExpenseCateList.Refresh();

        $('#ddlAppIds').on('change', function () {
            if ($("#ddlAppIds option:selected").index() > 0) {
                var selectedValue = $(this).val();
                $('#ddlAppIds').prop("selectedIndex", 0);
                var url = '@Url.Action("ExportExpenseCategorylist", "ExpenseCategory", new { type = "_type_" })'
                window.location.href = url.replace("_type_", selectedValue);
            }
        });
    });

function OnStartCallback(s, e) {

}

var chkArr = "";

function OpenExpencforEdit(obj) {
    //alert(obj);
    $("#ExpenseID").val(obj);
    $.ajax({
        type: "POST",
        url: "@Url.Action("ViewDataShow", "ExpenseCategory")",
        data: { ExpenseID: obj },
    dataType: "json",
    success: function (response) {
        var status = response;
        var str = "";

        if (status != null) {
            $("#Expense_Name").val(status.Expense_Name);
            $("#ddlExpenseType").val(status.Expense_Type);
            $("#ddlTRANSID").val(status.TransactionCategory);
            $("#ddlReceiptRequiredID").val(status.ReceiptReq);
            $("#ddlBranch").val(status.BRANCH);
            $("#expCate").modal('toggle');

            $("#btnSave").removeClass('hide');
            //$("#btnSave").attr("enabled", "enabled");
        }

    },
    error: function (response) {
        //  alert(response);
        jAlert("Please try again later");
        //LoadingPanel.Hide();
    }
});
}

function OpenExpenceforDelete(obj) {
    jConfirm('Confirm Delete?', 'Alert', function (r) {
        if (r) {
            $("#ExpenseID").val(obj);
            $.ajax({
                type: "POST",
                url: "@Url.Action("DeleteData", "ExpenseCategory")",
                data: { ExpenseID: obj },
            dataType: "json",
            success: function (response) {
                var status = response;
                jAlert(status);
                gridExpenseCateList.Refresh();
            },
            error: function (response) {
                // alert(response);
                jAlert("Please try again later.");
            }
        });
    }
    else {
    // alert("false");
}
});
}


function OpenExpencforView(obj) {
    //alert(obj);
    $("#ExpenseID").val(obj);
    $.ajax({
        type: "POST",
        url: "@Url.Action("ViewDataShow", "ExpenseCategory")",
        data: { ExpenseID: obj },
    dataType: "json",
    success: function (response) {
        var status = response;
        var str = "";

        if (status != null) {
            $("#Expense_Name").val(status.Expense_Name);
            $("#ddlExpenseType").val(status.Expense_Type);
            $("#ddlTRANSID").val(status.TransactionCategory);
            $("#ddlReceiptRequiredID").val(status.ReceiptReq);
            $("#ddlBranch").val(status.BRANCH);
            $("#btnSave").addClass('hide');
            $("#expCate").modal('toggle');
        }

    },
    error: function (response) {
        // alert(response);
        jAlert("Please try again later.");
    }
});
}

function ExpenceSave() {
    var obj = {};
    obj.ExpenseID = $("#ExpenseID").val();
    obj.Expense_Name = $("#Expense_Name").val().trim();

    var e = document.getElementById("ddlExpenseType");
    obj.Expense_Type = e.options[e.selectedIndex].value;
    var h = document.getElementById("ddlTRANSID");
    obj.TransactionCategory = h.options[h.selectedIndex].value;
    var j = document.getElementById("ddlReceiptRequiredID");
    obj.ReceiptReq = j.options[j.selectedIndex].value;
    var f = document.getElementById("ddlBranch");
    obj.BRANCH = f.options[f.selectedIndex].value;

    if (obj.Expense_Name != "") {
        if (obj.Expense_Type != "0") {
            if (obj.TransactionCategory != "0") {
                if (obj.ReceiptReq != "0") {
                    if (obj.BRANCH != "") {
                        LoadingPanel.Show();
                        $.ajax({
                            type: "POST",
                            url: "@Url.Action("SaveData", "ExpenseCategory")",
                            data: { Expenc: obj },
                        success: function (response) {
                            jAlert(response);
                            $("#expCate").modal('toggle');
                            LoadingPanel.Hide();
                            if (response == 'Saved Successfully.') {
                                $("#ExpenseID").val('');
                                $("#Expense_Name").val('');
                                $("#ddlExpenseType").val(0);
                                $("#ddlTRANSID").val(0);
                                $("#ddlBranch").val(0);
                                $("#ddlReceiptRequiredID").val(0);
                                $("#btnSave").removeClass('hide')
                                gridExpenseCateList.Refresh();
                            }
                        },
                        error: function (response) {
                            jAlert("Please try again later.", "Alert", function () {
                                setTimeout(function () {
                                    $('#Expense_Name').focus();
                                }, 200);
                            });
                            LoadingPanel.Hide();
                        }
                    });
                }
                else {
                    jAlert("Unit is Mandatory.", "Alert", function () {
                        setTimeout(function () {
                            $('#ddlBranch').focus();
                        }, 200);
                    });
                }
            }
            else {
                jAlert("Receipt Required is Mandatory.", "Alert", function () {
                    setTimeout(function () {
                        $('#ddlReceiptRequiredID').focus();
                    }, 200);
                });
            }
        }
        else {
            jAlert("Transaction Category is Mandatory.", "Alert", function () {
                setTimeout(function () {
                    $('#ddlTRANSID').focus();
                }, 200);
            });
        }
    }
    else {
        jAlert("Expense Type is Mandatory.", "Alert", function () {
            setTimeout(function () {
                $('#ddlExpenseType').focus();
            }, 200);
        });
    }
}
else {
            jAlert("Name is Mandatory.", "Alert", function () {
                setTimeout(function () {
                    $('#Expense_Name').focus();
                }, 200);
            });
}
}


function Close() {
    $("#ExpenseID").val('');
    $("#Expense_Name").val('');
    $("#ddlExpenseType").val(0);
    $("#ddlTRANSID").val(0);
    $("#ddlBranch").val(0);
    $("#ddlReceiptRequiredID").val(0);

    $("#btnSave").removeClass('hide');
}
function gridRowclick(s, e) {
    $('#gridExpenseCateList').find('tr').removeClass('rowActive');
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
                $(value).css({ 'opacity': '1' });
            }, 100);
        });
    }, 200);
}
