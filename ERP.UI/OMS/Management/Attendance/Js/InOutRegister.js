var Reportpage;
var EmpArr = new Array();
$(document).ready(function () {
    var EmpObj = new Object();
    EmpObj.Name = "EmpSource";
    EmpObj.ArraySource = EmpArr;
    arrMultiPopup.push(EmpObj);

    setTimeout(function () { cErrorCheck.PerformCallback(); }, 1000);
    


    $('#EmployeeModel').on('shown.bs.modal', function () {
        clearPopup();
        $('#txtEmpSearch').focus();
    }) 
})


function EmployeeSelect() {
    clearPopup();
    $('#EmployeeModel').modal('show');
}

function EmployeebtnKeyDown(s,e) {
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
    if (Reportpage)
        Reportpage.close();
    if (!chkAllEmp.GetChecked() && GetObjectID('EmpId').value == '') {
        jAlert('Please Select an Employee.', "Alert", function () {
            EmployeeSelect();
        });
        return false;
    }
    var showInactive = 1;
    if (!cchkInactive.GetChecked()) {
        showInactive = 0;
    }

    //localStorage.setItem("attRptEmpId", GetObjectID('EmpId').value);
    //localStorage.setItem("attRptBranch", ccmbBranch.GetValue());
    //localStorage.setItem("attRptBranchName", ccmbBranch.GetText());
    //localStorage.setItem("showInactive", showInactive);
    //localStorage.setItem("attRptFromDate", cFormDate.GetDate().format('yyyy-MM-dd')); 
    //Reportpage = window.open('allEmpAttRecords.aspx', '_blank');


    cGridDetail.PerformCallback();
}

function allEmpChange(s, e) {
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


 

function gridBeginCallBack() {
    $('#drdExport').val(0);
}


function navOnOff() {

    if ($('#rightNav').hasClass('off')) {
        $('#rightNav').removeClass('off');
        $('#rightNav').addClass('on');
    } else {
        $('#rightNav').removeClass('on');
        $('#rightNav').addClass('off');
    }

}


google.charts.load('current', { packages: ["orgchart"] }); 

function rowDbClick(s,e) {
    data = new google.visualization.DataTable();
    data.addColumn('string', 'Name');
    data.addColumn('string', 'Manager');
    data.addColumn('string', 'ToolTip');

    if (cGridDetail.GetRow(e.visibleIndex).children[4].innerText.trim() != '') {
        data.addRow([cGridDetail.GetRow(e.visibleIndex).children[4].innerText.trim(), '', '']);

        for (var i = 1; i < 20; i++) {

            if (cGridDetail.GetRow(e.visibleIndex).children[4 + i].innerText.trim() == "")
                break;

            data.addRow([cGridDetail.GetRow(e.visibleIndex).children[4 + i].innerText.trim(), cGridDetail.GetRow(e.visibleIndex).children[3 + i].innerText.trim(), '']);
        }
        var chart = new google.visualization.OrgChart(document.getElementById('HierchyDiv'));
        chart.draw(data, { allowHtml: true });


        if ($('#rightNav').hasClass('off')) {
            $('#rightNav').removeClass('off');
            $('#rightNav').addClass('on');
        }
    } else {
        if ($('#rightNav').hasClass('on')) {
            $('#rightNav').removeClass('on');
            $('#rightNav').addClass('off');
        }
    }

   
    


}