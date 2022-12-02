var ListOfEmail = [];
var ListOfCCEmail = [];
var EorC = '';

function cmbMainBranchChange(s, e) {

    var OtherDetails = {}
    OtherDetails.BranchId = ccmbMainUnit.GetValue();
    $.ajax({
        type: "POST",
        url: "DailyAttRegister.aspx/GetAllDetailsByBranch",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var returnObject = msg.d;

            SetDataSourceOnComboBox(ccmbSubunit, returnObject);

        }
    });

}

function SetDataSourceOnComboBox(ControlObject, Source) {
    ControlObject.ClearItems();
    for (var count = 0; count < Source.length; count++) {
        ControlObject.AddItem(Source[count].Name, Source[count].Id);
    }
    ControlObject.SetSelectedIndex(0);
}


function ShowAttendance() {
    if (ccmbMainUnit.GetText() == "") {
        jAlert("Please Select Unit.")
    } else {
        setBranch();
        $('#hdShowInactive').val('0');
        if (document.getElementById('chkExcludeEmp').checked) {
            $('#hdShowInactive').val('1');
        }


        cgridAttendance.Refresh();
    }
}

function onBeginCallBack() {
    $('#drdExport').val(0);
}



function setBranch() {
    if (ccmbSubunit.GetValue() == "0") {
        var BranchList = ccmbMainUnit.GetValue();
        for (var i = 0; i < ccmbSubunit.GetItemCount() ; i++) {
            BranchList = BranchList + ',' + ccmbSubunit.GetItem(i).value
        }
        //BranchList = BranchList.substr(1, BranchList.length);
        GetObjectID('hdBranchList').value = BranchList;
    }
    else {
        GetObjectID('hdBranchList').value = ccmbSubunit.GetValue();
    }
}


function OpenMailPopup() {
    if (cgridAttendance.GetVisibleRowsOnPage() == 0) {
        jAlert("Its seems that you have not Generated the Daily Attendance Register Report, Please Generate it before using the Email feature.")
    } else {
        cmailSubject.SetText('Daily Attendance Register [' + ctoDate.GetDate().format('dd-MM-yyyy') + ']')


        cMailBody.SetText('Please find the Attendance Register attached herewith this mail\ndated:'+ ctoDate.GetDate().format('dd-MM-yyyy')+' for the Unit:'+ccmbSubunit.GetText()+'.')

        $('#MailIt').modal('show');
    }
}


function SendMailNow() {
    LoadMailTo();
    LoadCCMailTo();
    cgridAttendance.PerformCallback('SendMail');
}

function LoadMailTo() {
    var MailList = "";
    for (var x = 0; x < document.getElementById('listOfEmail').children.length; x++) {
        MailList = MailList + document.getElementById('listOfEmail').children[x].innerHTML.split('</button>')[1] + ";";
    }

    $('#MailTO').val(MailList);
}


function LoadCCMailTo() {
    var MailList = "";
    for (var x = 0; x < document.getElementById('listOfCCmail').children.length; x++) {
        MailList = MailList + document.getElementById('listOfCCmail').children[x].innerHTML.split('</button>')[1] + ";";
    }

    $('#CCMail').val(MailList);
}

function gridEndCallBack() {

    if(cgridAttendance.cpCallPara)
        if (cgridAttendance.cpCallPara == 'SendMail') {
            cgridAttendance.cpCallPara = null;
            jAlert(cgridAttendance.cpRetMsg);

            $('#MailIt').modal('hide');            
            cgridAttendance.cpRetMsg = null;
        }

}


function EmployeeSelect(eorC) {
    EorC = eorC;
    $('#EmailModel').modal('show');
}

function EmployeebtnKeyDown(s,e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        EmployeeSelect();
    }
}


function EmailKeyDown(e) {

    var OtherDetails = {}
    OtherDetails.SerarchKey = $("#emailtxt").val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = []; 
        HeaderCaption.push("Email");
        if ($("#txtEmpSearch").val() != '') {
            callonServer("DailyAttRegister.aspx/GetEmail", OtherDetails, "EmployeeTable", HeaderCaption, "EmployeeIndex", "SetEmployee");

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
    $('#EmailModel').modal('hide');
    if (EorC == 'E') {
        ListOfEmail.push(id);
        bindEmail();
    }
    else {
        ListOfCCEmail.push(id);
        bindccEmail();
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

            $('#emailtxt').focus();
        }
    }

}


function bindEmail() {

    $('#listOfEmail').html('');
    for (var i = 0 ; i < ListOfEmail.length; i++) {
        //var dyHtml = '<span class="alert alert-success fade in alert-dismissible" style="padding: 4px; margin: 5px;">';
        var dyHtml = '<span class="xxxx">';
        dyHtml = dyHtml + ' <button type="button" class="close" style="position: absolute" aria-label="Close" id="closeAlert" onclick=RemoveEmail("' + ListOfEmail[i] + '")>';
        dyHtml= dyHtml + '   <span aria-hidden="true" style="font-size: 20px">×</span>';
        dyHtml = dyHtml + '</button>';
        dyHtml= dyHtml +  ListOfEmail[i] + '</span>';
       
                                  
                               
        $('#listOfEmail').append(dyHtml)
    }

}


function bindccEmail() {

    $('#listOfCCmail').html('');
    for (var i = 0 ; i < ListOfCCEmail.length; i++) {
        //var dyHtml = '<span class="alert alert-success fade in alert-dismissible" style="padding: 4px; margin: 5px;">';
        var dyHtml = '<span class="xxxx">';
        dyHtml = dyHtml + ' <button type="button" class="close" style="position: absolute" aria-label="Close" id="closeAlert" onclick=RemoveCCEmail("' + ListOfCCEmail[i] + '")>';
        dyHtml = dyHtml + '   <span aria-hidden="true" style="font-size: 20px">×</span>';
        dyHtml = dyHtml + '</button>';
        dyHtml = dyHtml + ListOfCCEmail[i] + '</span>';



        $('#listOfCCmail').append(dyHtml)
    }

}


function RemoveEmail(email) {
    var index = ListOfEmail.indexOf(email);

    if (index > -1) {
        ListOfEmail.splice(index, 1);
    } 
    bindEmail();
}

function RemoveCCEmail(email) {
    var index = ListOfCCEmail.indexOf(email);

    if (index > -1) {
        ListOfCCEmail.splice(index, 1);
    }
    bindccEmail();
}


function ExEmpchkChange() {
    if (document.getElementById('chkExcludeEmp').checked) {
        $('#lblexclude').text('Include Inactive Employee(s)');
    } else {
        $('#lblexclude').text('Exclude Inactive Employee(s)');
    }
}
