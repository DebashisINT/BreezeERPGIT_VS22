
function Employeekeydown(e) {

    var OtherDetails = {}
    OtherDetails.SerarchKey = $("#txtEmpSearch").val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Employee Name");
        HeaderCaption.push("Employee Id");
        if ($("#txtEmpSearch").val() != '') {
            callonServer("Service/AttdendanceService.asmx/GetEmployee", OtherDetails, "EmployeeTable", HeaderCaption, "EmployeeIndex", "SetEmployee");

            e.preventDefault();
            return false;
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[EmployeeIndex=0]"))
            $("input[EmployeeIndex=0]").focus();
    }

}
function SetEmployee(id, name) {
    $('#EmployeeModel').modal('hide');
    cempButtonEdit.SetText(name);
    $('#EmpId').val(id);
}
function ShowAttendance() {

    if ($('#EmpId').val().trim() == "") {
        jAlert("Please Select an Employee.", "Alert", function () {
            EmployeeSelect();
        });
    } else {
        updateChart.PerformCallback();
        updateChart.PerformCallback();
    }
}
function MyOnInit() {

}


function ValueSelected(e, indexName) {
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var Id = e.target.parentElement.parentElement.cells[0].innerText;
        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
        if (Id) {
            SetEmployee(Id, name);
        }

    }

    else if (e.code == "ArrowDown") {
        thisindex = parseFloat(e.target.getAttribute(indexName));
        thisindex++;
        if (thisindex < 10)
            $("input[" + indexName + "=" + thisindex + "]").focus();
    }
    else if (e.code == "ArrowUp") {
        thisindex = parseFloat(e.target.getAttribute(indexName));
        thisindex--;
        if (thisindex > -1)
            $("input[" + indexName + "=" + thisindex + "]").focus();
        else {

            $('#txtEmpSearch').focus();
        }
    }

}
function EmployeeSelect() {
    $('#EmployeeModel').modal('show');
}
function EmployeeKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        EmployeeSelect();
    }
}

 

$(document).ready(function () {
    $('#EmployeeModel').on('shown.bs.modal', function () { 
        $('#txtEmpSearch').focus();
    })
    $('#EmployeeModel').on('hidden.bs.modal', function () {
        if ($('#EmpId').val().trim() == "") {
            cempButtonEdit.Focus();
        } else {
            $('#BtnShow').focus();
        }
    })

   
    ccmbMonthYear.Focus();
    //$('#BtnShowEmployee').focus();
})



 