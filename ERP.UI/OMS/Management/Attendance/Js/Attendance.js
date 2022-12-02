var weekday = new Array(7);
weekday[0] = "Sunday";
weekday[1] = "Monday";
weekday[2] = "Tuesday";
weekday[3] = "Wednesday";
weekday[4] = "Thursday";
weekday[5] = "Friday";
weekday[6] = "Saturday";


var MonthDay = new Array(12);
MonthDay[0] = "January";
MonthDay[1] = "February";
MonthDay[2] = "March";
MonthDay[3] = "April";
MonthDay[4] = "May";
MonthDay[5] = "June";
MonthDay[6] = "July";
MonthDay[7] = "Auguest";
MonthDay[8] = "September";
MonthDay[9] = "October";
MonthDay[10] = "November";
MonthDay[11] = "December";




function updateTime() {
    var currentTime = new Date()
    var hours = currentTime.getHours()
    var minutes = currentTime.getMinutes()
    if (minutes < 10) {
        minutes = "0" + minutes
    }
    var t_str;

    if (hours > 11) { 
        hours = hours - 12;
        t_str = hours + ":" + minutes + ":" + currentTime.getSeconds() + " PM";
        
    } else {
        t_str = hours + ":" + minutes + ":" + currentTime.getSeconds() + " AM";
        
    }

    


    document.getElementById('Day_span').innerHTML = weekday[currentTime.getDay()] + " , " + MonthDay[currentTime.getMonth()] + " " + currentTime.getDate() + " , " + currentTime.getFullYear() + "<span style='color: red;font-size: 13px;font-size: 13px;'> *</span>";
    document.getElementById('time_span').innerHTML = t_str;
}


setInterval(updateTime, 1000);




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
    document.getElementById('EmployeeNameSpan').innerText = name;
    $('#EmpId').val(id);
}

function EmployeeSelect() {
    $('#EmployeeModel').modal('show');
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

function AttendanceSubmit() {
    if ($('#EmpId').val().trim() == "") {
        jAlert('Please Select an Employee.', "Alert", function () {
                $('#BtnShowEmployee').click(); 
        });
    }
    else {
        var OtherDetails = {}
        OtherDetails.EmpId = $("#EmpId").val();
        $.ajax({ 
            type: "POST",
            url: "Service/AttdendanceService.asmx/SaveAttendance",
            data: JSON.stringify(OtherDetails),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                jAlert(msg.d.Msg, "Alert", function (a) {
                    setTimeout(function () {
                        $('#BtnShowEmployee').focus();
                    },200);
                });

                if (msg.d.status == "Ok") {
                    $('#EmpId').val('');
                    document.getElementById('EmployeeNameSpan').innerText = '';
                }
            }

        });


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
            $('#BtnShowEmployee').focus();
        } else {
            $('#BtnSubmitRequest').focus();
        }
    })

    document.getElementById('EmployeeNameSpan').innerText = $('#hdEmpName').val();
   

    $('#BtnSubmitRequest').focus();
})

 