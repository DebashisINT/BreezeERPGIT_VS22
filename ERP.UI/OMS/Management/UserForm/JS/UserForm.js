var HeaderObj;

function ValidateHeader() {
    HeaderObj = JSON.parse($('#HeaderJson').text());

    var CurObj;
    for (var i = 0 ; i < HeaderObj.length; i++) {
        CurObj = HeaderObj[i];
        if (CurObj.Mandatory) {
            if (CurObj.controlType == 'Text Field' || CurObj.controlType == 'Date' || CurObj.controlType == 'Formula') {
                objCtrl = ASPxClientControl.GetControlCollection().GetByName('cid' + CurObj.Id);
                if (objCtrl.GetText().trim() == '') {
                    jAlert("Please fill All mandatory Field.", "Alert", function () {
                        objCtrl.Focus();
                    });
                    return false;
                }
            }
            else if (CurObj.controlType == 'Numeric') {
                objCtrl = ASPxClientControl.GetControlCollection().GetByName('cid' + CurObj.Id);
                if (objCtrl.GetValue()==0) {
                    jAlert("Please fill All mandatory Field.", "Alert", function () {
                        objCtrl.Focus();
                    });
                    return false;
                }
            }
            else if (CurObj.controlType == 'Customer Master') {
                if (ctxtCustName.GetText() == "") {
                    jAlert("Please fill All mandatory Field.", "Alert", function () {
                        ctxtCustName.Focus();
                    });
                    return false;
                }

            }

            else if (CurObj.controlType == 'Employee Master') {
                if (ctxtEmployeeName.GetText() == "") {
                    jAlert("Please fill All mandatory Field.", "Alert", function () {
                        ctxtEmployeeName.Focus();
                    });
                    return false;
                }

            }


            else if (CurObj.controlType == 'Drop Down') {
                objCtrl = ASPxClientControl.GetControlCollection().GetByName('cid' + CurObj.Id);
                if (objCtrl.GetText() == "") {
                    jAlert("Please fill All mandatory Field.", "Alert", function () {
                        objCtrl.Focus();
                    });
                    return false;
                }
            }




        }

    }
    return true;
}


function ValueSelected(e, indexName) {
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var Id = e.target.parentElement.parentElement.cells[0].innerText;
        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
        if (Id) {
            if (indexName == "customerIndex") {
                SetCustomer(Id, name);
            }
            else if (indexName == "EmployeeIndex") {
                SetEmployee(Id, name);
            }
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
            if (indexName == "ProdIndex")
                $('#txtProdSearch').focus();
            else if (indexName == "customerIndex") {
                $('#txtCustSearch').focus();
            }
            else if (indexName == "EmployeeIndex") {
                $('#txtEmployeeSearch').focus();
            }
            else
                $('#txtCustSearch').focus();
        }
    }

}

function onSaveClick() {
    var SaveDetails = [];
    var SaveObj = {};

    if (ValidateHeader() == false) {
        return;
    }


    HeaderObj = JSON.parse($('#HeaderJson').text());
    for (var i = 0 ; i < HeaderObj.length; i++) {
        CurObj = HeaderObj[i];
        if (CurObj.controlType == 'Text Field' || CurObj.controlType == 'Formula' || CurObj.controlType == 'Memo Field') {
            objCtrl = ASPxClientControl.GetControlCollection().GetByName('cid' + CurObj.Id);
            SaveObj = {};
            SaveObj.ColumnName = HeaderObj[i].FieldName;
            SaveObj.Result = objCtrl.GetText();
            SaveDetails.push(SaveObj);
        }
        
        else if (CurObj.controlType == 'Drop Down') {
            objCtrl = ASPxClientControl.GetControlCollection().GetByName('cid' + CurObj.Id);
            SaveObj = {};
            SaveObj.ColumnName = HeaderObj[i].FieldName;
            SaveObj.Result = objCtrl.GetText();
            SaveDetails.push(SaveObj);
        }

        else if (CurObj.controlType == 'Numeric') {
            objCtrl = ASPxClientControl.GetControlCollection().GetByName('cid' + CurObj.Id);
            SaveObj = {};
            SaveObj.ColumnName = HeaderObj[i].FieldName;
            SaveObj.Result = objCtrl.GetValue();
            SaveDetails.push(SaveObj);
        }
        else if (CurObj.controlType == 'Date') {
            objCtrl = ASPxClientControl.GetControlCollection().GetByName('cid' + CurObj.Id);
            SaveObj = {};
            SaveObj.ColumnName = HeaderObj[i].FieldName;
            SaveObj.Result = objCtrl.GetDate().format("yyyy-MM-dd");
            SaveDetails.push(SaveObj);
        }

        else if (CurObj.controlType == 'Customer Master') { 
            SaveObj = {};
            SaveObj.ColumnName = HeaderObj[i].FieldName;
            SaveObj.Result = ctxtCustName.GetText();
            SaveDetails.push(SaveObj);

            SaveObj = {};
            SaveObj.ColumnName = HeaderObj[i].FieldName+'_Value';
            SaveObj.Result = $('#HdCustId').val();
            SaveDetails.push(SaveObj);
        }

        else if (CurObj.controlType == 'Employee Master') {
            SaveObj = {};
            SaveObj.ColumnName = HeaderObj[i].FieldName;
            SaveObj.Result = ctxtEmployeeName.GetText();
            SaveDetails.push(SaveObj);

            SaveObj = {};
            SaveObj.ColumnName = HeaderObj[i].FieldName + '_Value';
            SaveObj.Result = $('#HdEmpId').val();
            SaveDetails.push(SaveObj);
        }
         
    }

    var OtherDet = {};
    OtherDet.SaveDetails = SaveDetails;
    OtherDet.ModuleName = $('#hdModuleName').val();
    OtherDet.hdtagId = $('#hdtagId').val();

    $.ajax({
        type: "POST",
        url: "UserForm.aspx/AddField",
        data: JSON.stringify(OtherDet),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
           
            if (msg.d.split('~')[0] == '0') {
                jAlert(msg.d.split('~')[1], 'Alert', function () {
                    onBack();
                });
                
            }
            else {
                jAlert(msg.d.split('~')[1]);
            }

        }
    });
     
}


function onBack() {
    window.location.href = 'userformList.aspx?ModName=' + $('#hdModuleName').val()
}




function SetFormula() {
    HeaderObj = JSON.parse($('#HeaderJson').text());

    var FormulObj = $.grep(HeaderObj, function (e) { return e.Formula && e.Formula != ""; })
    for (var i = 0 ; i< FormulObj.length; i++) {
        var Formula = FormulObj[i].Formula;
        
        for (var fr = 0; fr < HeaderObj.length; fr++) {
            if (Formula.indexOf('[' + HeaderObj[fr].FieldName + ']') != -1) {
                objCtrl = ASPxClientControl.GetControlCollection().GetByName('cid' + HeaderObj[fr].Id);
                Formula = Formula.replace('[' + HeaderObj[fr].FieldName + ']', objCtrl.GetText());
                Formula = Formula.replace('[' + HeaderObj[fr].FieldName + ']', objCtrl.GetText());
                Formula = Formula.replace('[' + HeaderObj[fr].FieldName + ']', objCtrl.GetText());
            }
        }

        objCtrl = ASPxClientControl.GetControlCollection().GetByName('cid' + FormulObj[i].Id);
        objCtrl.SetText(eval(Formula));
    }


}