
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
        // cgridAttendance.PerformCallback();
        cgridAttendance.Refresh();
        cgridAttendance.Refresh();
    }
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


function clearPopup() {

    var rowsArr = $('.dynamicPopupTbl')[0].rows;
    var len = rowsArr.length;
    while (rowsArr.length > 1) {
        rowsArr[rowsArr.length - 1].remove();
    }

    $('#txtEmpSearch').val('');

}

$(document).ready(function () {
    $('#EmployeeModel').on('shown.bs.modal', function () {
        clearPopup();
        $('#txtEmpSearch').focus();
    })
    $('#EmployeeModel').on('hidden.bs.modal', function () {
        if ($('#EmpId').val().trim() == "") {
            cempButtonEdit.Focus();
        } else {
            $('#BtnShow').focus();
        }
    })

    $('#TimeUpdateModel').on('shown.bs.modal', function () {
        cEmpStatus.Focus();
        StatusChange();
    })
    ccmbMonthYear.Focus();
    //$('#BtnShowEmployee').focus();


})




function GridEndCallback() {
    if (cgridAttendance.cpReturnMsg) {
        if (cgridAttendance.cpReturnMsg.split('~')[0] == "Update") {
            if (cgridAttendance.cpReturnMsg.split('~')[1] == "MustBeGreater.") {

                jAlert("Out-Time must be later than In-Time.");
            } else {
                jAlert(cgridAttendance.cpReturnMsg.split('~')[1]);
                $('#TimeUpdateModel').modal('hide');
            }
        }
    }
}

function onEditClick(vIndex, KeyValue, Attstatus, Remarks) {
    //Remarks = Remarks.replace("|~|", '"');
    var _Remarks = "";
    var SpliteDetails = Remarks.split("|@|");
    for (index = 0; index < SpliteDetails.length; index++) {
        if (index == 0) _Remarks = SpliteDetails[index];
        else _Remarks = _Remarks + '"' + SpliteDetails[index];
    }

    Remarks = _Remarks;
    _Remarks = "";
    var SpliteDetails = Remarks.split("~");
    for (index = 0; index < SpliteDetails.length; index++) {
        if (index == 0) _Remarks = SpliteDetails[index];
        else _Remarks = _Remarks + "'" + SpliteDetails[index];
    }

    Remarks = _Remarks;

    cEmpStatus.SetValue(Attstatus)
    $('#KeyValue').val(KeyValue);
    ctxtRewmarks.SetText(Remarks);
    var AttDay = cgridAttendance.GetRow(vIndex).children[0].innerText.trim();
    $('#hdAttDate').val(AttDay);


    var inTime = cgridAttendance.GetRow(vIndex).children[1].innerText.trim();
    var outTime = cgridAttendance.GetRow(vIndex).children[2].innerText.trim();

    if (inTime == "")
        cIntimeEdit.SetDate(null);
    else {

        var inHour = parseFloat(inTime.split(':')[0]);
        if (inTime.includes('AM') && inHour == 12)
            inHour = 0
        else if (inTime.includes('PM') && inHour != 12)
            inHour += 12;

        var inDt = new Date(parseFloat(AttDay.split('-')[2]), parseFloat(AttDay.split('-')[1]), parseFloat(AttDay.split('-')[0]),
           inHour, parseFloat(inTime.split(':')[1]), parseFloat(inTime.split(':')[2]), 0);
        cIntimeEdit.SetDate(inDt);
    }


    if (outTime == "")
        cOutTimeEdit.SetDate(null);
    else {
        var outHour = parseFloat(outTime.split(':')[0]);

        if (outTime.includes('AM') && outHour == 12)
            outHour = 0
        else if (outTime.includes('PM') && outHour != 12)
            outHour += 12;

        var outDt = new Date(parseFloat(AttDay.split('-')[2]), parseFloat(AttDay.split('-')[1]), parseFloat(AttDay.split('-')[0]),
           outHour, parseFloat(outTime.split(':')[1]), parseFloat(outTime.split(':')[2]), 0);
        cOutTimeEdit.SetDate(outDt);
    }
    $('#TimeUpdateModel').modal('show');

}

function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
}

function UpdateTime() {
    var remarks = ctxtRewmarks.GetText().trim();
    if (remarks == "") {
        $("#txt_rmrks").show();
        return false;
    }
    else {
        $("#txt_rmrks").hide();
        cgridAttendance.PerformCallback('Update');
    }

}

document.onkeydown = function (e) {
    if ($('#TimeUpdateModel')[0].style.display == "block" && event.keyCode == 83 && event.altKey == true) {
        StopDefaultAction(e);
        UpdateTime();
    }
    else if ($('#TimeUpdateModel')[0].style.display == "block" && event.keyCode == 67 && event.altKey == true) {
        StopDefaultAction(e);
        $('#TimeUpdateModel').modal('hide');
    }
}



function StatusChange() {
    if (cEmpStatus.GetValue() == 'AB' || cEmpStatus.GetValue() == 'CO' || cEmpStatus.GetValue() == 'OVOT' || cEmpStatus.GetValue() == 'PH' || cEmpStatus.GetValue() == 'PL' || cEmpStatus.GetValue() == 'SL' || cEmpStatus.GetValue() == 'WO') {
        cIntimeEdit.SetValue(null);
        cOutTimeEdit.SetValue(null);
        cIntimeEdit.SetEnabled(false);
        cOutTimeEdit.SetEnabled(false);
    } else {
        cIntimeEdit.SetEnabled(true);
        cOutTimeEdit.SetEnabled(true);
    }
}



function onBeginCallBack() {
    $('#drdExport').val(0);
}