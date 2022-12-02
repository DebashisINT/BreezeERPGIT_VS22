var Reportpage;
var EmpArr = new Array();
$(document).ready(function () {
    var EmpObj = new Object();
    EmpObj.Name = "EmpSource";
    EmpObj.ArraySource = EmpArr;
    arrMultiPopup.push(EmpObj);

    $('#EmployeeModel').on('shown.bs.modal', function () {
        clearPopup();
        $('#txtEmpSearch').focus();
    })
    FromDateChange();
})


function EmployeeSelect() {
    clearPopup();
    $('#EmployeeModel').modal('show');
}

function EmployeebtnKeyDown() {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        EmployeeSelect();
    }
}

function Employeekeydown(e) {
    var OtherDetails = {}

    if ($.trim($("#txtEmpSearch").val()) == "" || $.trim($("#txtEmpSearch").val()) == null) {
        return false;
    }
    OtherDetails.SerarchKey = $("#txtEmpSearch").val();
    OtherDetails.BranchId = ccmbBranch.GetValue();


    if (e.code == "Enter" || e.code == "NumpadEnter") { 
        var HeaderCaption = [];
        HeaderCaption.push("Employee Name");
        HeaderCaption.push("Employee Id");


        if ($("#txtEmpSearch").val() != "") { 
            callonServerM("Service/AttdendanceService.asmx/GetEmployeeByBranch", OtherDetails, "EmployeeTable", HeaderCaption, "EmployeeIndex", "SetSelectedValues", "EmpSource");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[EmployeeIndex=0]"))
            $("input[EmployeeIndex=0]").focus();
    }

}

function SetSelectedValues(Id, Name, ArrName) {
    if (ArrName == 'EmpSource') {
        var key = Id;
        if (key != null && key != '') {
            $('#EmployeeModel').modal('hide');
            cempButtonEdit.SetText(Name);
            GetObjectID('EmpId').value = key;
        }
        else {
            cempButtonEdit.SetText('');
            GetObjectID('EmpId').value = '';
        }
    }

}



function ShowReport() {
    if(Reportpage)
        Reportpage.close();
    if (!chkAllEmp.GetChecked() &&  GetObjectID('EmpId').value == '') {
        jAlert('Please Select an Employee.', "Alert", function () {
            EmployeeSelect();
        });
        return false;
    }
    var showInactive = 1;
    if (!cchkInactive.GetChecked()) {
        showInactive = 0;
    }

    localStorage.setItem("attRptEmpId", GetObjectID('EmpId').value); 
    localStorage.setItem("attRptBranch", ccmbBranch.GetValue());
    localStorage.setItem("attRptBranchName", ccmbBranch.GetText());
    localStorage.setItem("showInactive", showInactive);
    localStorage.setItem("attRptFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
    localStorage.setItem("attRptToDate", cToDate.GetDate().format('yyyy-MM-dd'));
    if (cchkPayrollBranch.GetChecked())
        localStorage.setItem("attConsiderPayBranch", "1");
    else
        localStorage.setItem("attConsiderPayBranch", "0");

    Reportpage = window.open('allEmpAttRecords.aspx', '_blank');
}

function allEmpChange(s,e) {
    if (chkAllEmp.GetChecked()) {
        cempButtonEdit.SetEnabled(false);
        cempButtonEdit.SetText('');
        GetObjectID('EmpId').value = '';
        arrMultiPopup[0].ArraySource = [];
    } else {
        cempButtonEdit.SetEnabled(true);
    }

}

function cmbBranchChange() {
    
    if (!chkAllEmp.GetChecked()) {
        cempButtonEdit.SetText('');
        GetObjectID('EmpId').value = '';
        arrMultiPopup[0].ArraySource = [];
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



function FromDateChange(s,e){
    var date = cFormDate.GetDate();
    var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
    cToDate.SetMaxDate(lastDay);
    cToDate.SetDate(date);
    cToDate.SetMinDate(cFormDate.GetDate());
}