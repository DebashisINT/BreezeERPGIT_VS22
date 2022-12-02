
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

function EmployeeSelect() {
    $('#EmployeeModel').modal('show');
}

function EmployeeKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        EmployeeSelect();
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

        $("#Workingdays").removeClass("MakeRound");
        $("#co").removeClass("MakeRound");
        $("#hc").removeClass("MakeRound");
        $("#OV").removeClass("MakeRound");
        $("#PD").removeClass("MakeRound");
        $("#PH").removeClass("MakeRound");
        $("#PL").removeClass("MakeRound");
        $("#SL").removeClass("MakeRound");
        $("#OVOT").removeClass("MakeRound");
        $("#HS").removeClass("MakeRound");


        $("#Workingdays").removeClass("count");
        $("#co").removeClass("count");
        $("#hc").removeClass("count");
        $("#OV").removeClass("count");
        $("#PD").removeClass("count");
        $("#PH").removeClass("count");
        $("#PL").removeClass("count");
        $("#SL").removeClass("count");
        $("#OVOT").removeClass("count");
        $("#HS").removeClass("count");


        cGrid.PerformCallback('2018');
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
function clearPopup() {

    var rowsArr = $('.dynamicPopupTbl')[0].rows;
    var len = rowsArr.length;
    while (rowsArr.length > 1) {
        rowsArr[rowsArr.length - 1].remove();
    }

    $('#txtEmpSearch').val('');

}


function gridEndCallback() {
    if (cGrid.cpHoliday) {
        var holidaysList = cGrid.cpHoliday;
        cGrid.cpHoliday = null;
        var leaveObj = JSON.parse(holidaysList)
        for (var i = 0; i<leaveObj.length; i++){

            if (leaveObj[i].Neme == "Co") {

                if (leaveObj[i].ThisYear == "0") {
                    $('#mainco').hide();
                } else
                    $('#mainco').show();

                document.getElementById('co').innerText = leaveObj[i].ThisYear;
                document.getElementById('lastco').innerText = leaveObj[i].lastYear;
                $("#co").addClass("MakeRound count");
            }
            else if (leaveObj[i].Neme == "Workingdays") {

                if (leaveObj[i].ThisYear == "0") {
                    $('#mainWorkingdays').hide();
                } else
                    $('#mainWorkingdays').show();

                $("#Workingdays").addClass("MakeRound");
                $("#Workingdays").addClass("count");
               

               document.getElementById('Workingdays').innerText = leaveObj[i].ThisYear;
                 document.getElementById('lastWorkingdays').innerText = leaveObj[i].lastYear;
               
                 
            }
            else if (leaveObj[i].Neme == "HC") {
                if (leaveObj[i].ThisYear == "0") {
                    $('#mainhc').hide();
                } else
                    $('#mainhc').show()

                $("#hc").addClass("MakeRound");

                document.getElementById('hc').innerText = leaveObj[i].ThisYear;
                document.getElementById('lasthc').innerText = leaveObj[i].lastYear;

            }
            else if (leaveObj[i].Neme == "HS") {
                if (leaveObj[i].ThisYear == "0") {
                    $('#mainHS').hide();
                } else
                    $('#mainHS').show()

                $("#HS").addClass("MakeRound");

                document.getElementById('HS').innerText = leaveObj[i].ThisYear;
                document.getElementById('lastHS').innerText = leaveObj[i].lastYear;
                  
            }
            else if (leaveObj[i].Neme == "OV") {

                if (leaveObj[i].ThisYear == "0") {
                    $('#mainOV').hide();
                } else
                    $('#mainOV').show();

                $("#OV").addClass("MakeRound");

                document.getElementById('OV').innerText = leaveObj[i].ThisYear;
                document.getElementById('lastOV').innerText = leaveObj[i].lastYear;
                 
            }
            else if (leaveObj[i].Neme == "PD") {

                if (leaveObj[i].ThisYear == "0") {
                    $('#mainPD').hide();
                } else
                    $('#mainPD').show();


                $("#PD").addClass("MakeRound");

                document.getElementById('PD').innerText = leaveObj[i].ThisYear;
                document.getElementById('lastPD').innerText = leaveObj[i].lastYear;
                
            }
            else if (leaveObj[i].Neme == "PH") {

                if (leaveObj[i].ThisYear == "0") {
                    $('#mainPH').hide();
                } else
                    $('#mainPH').show();


                $("#PH").addClass("MakeRound");

                document.getElementById('PH').innerText = leaveObj[i].ThisYear;
                document.getElementById('lastPH').innerText = leaveObj[i].lastYear;
                
            }
            else if (leaveObj[i].Neme == "PL") {

                if (leaveObj[i].ThisYear == "0") {
                    $('#mainPL').hide();
                } else
                    $('#mainPL').show();


                $("#PL").addClass("MakeRound");

                document.getElementById('PL').innerText = leaveObj[i].ThisYear;
                document.getElementById('lastPL').innerText = leaveObj[i].lastYear;
             
            }
            else if (leaveObj[i].Neme == "SL") {

                if (leaveObj[i].ThisYear == "0") {
                    $('#mainSL').hide();
                } else
                    $('#mainSL').show();

                $("#SL").addClass("MakeRound");

                document.getElementById('SL').innerText = leaveObj[i].ThisYear;
                document.getElementById('lastSL').innerText = leaveObj[i].lastYear;
                
            }
            //else if (leaveObj[i].Neme == "WO") {
            //    document.getElementById('WO').innerText = leaveObj[i].ThisYear;
            //    document.getElementById('lastWO').innerText = leaveObj[i].lastYear;
            //    if (leaveObj[i].ThisYear == "0") {
            //        $('#mainWO').hide();
            //    } else
            //        $('#mainWO').show()
            //}
            else if (leaveObj[i].Neme == "OVOT") {

                if (leaveObj[i].ThisYear == "0") {
                    $('#mainOVOT').hide();
                } else
                    $('#mainOVOT').show();


                $("#OVOT").addClass("MakeRound");

                document.getElementById('OVOT').innerText = leaveObj[i].ThisYear;
                document.getElementById('lastOVOT').innerText = leaveObj[i].lastYear;
               
            }
        }
        //document.getElementById('workingdaysId').innerText = spt[0];
        doRollOver();
    }

}
 
function doRollOver() {
    $('.count').each(function () {
        $(this).prop('Counter', 0).animate({
            Counter: $(this).text()
        }, {
            duration: 3500,
            easing: 'swing',
            step: function (now) {
                $(this).text(Math.ceil(now));
            }
        });
    });


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

    
})


