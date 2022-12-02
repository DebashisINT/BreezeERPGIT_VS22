
function SaveNew() {

    if (cFiledType.GetValue() == null) {
        jAlert("Please select Field Type.", "Alert", function () {
            cFiledType.Focus();
        });
        return false;
    }
    if (ctxtName.GetText().trim() == "") {
        jAlert("Please provide a Name.", "Alert", function () {
            ctxtName.Focus();
        });
        return false;
    }

    if (cFiledType.GetText().trim() == "Drop Down" && ctxtValues.GetText().trim() == "") {
        jAlert("you must provide value(s) for field type Drop Down.", "Alert", function () {
            ctxtValues.Focus();
        });
        return false;
    }


    var ModDet = {}
    var OtherDet = {};
    
    ModDet.ModuleId = $('#ModuleId').val();
    ModDet.FiledType = cFiledType.GetValue();
    ModDet.Name = ctxtName.GetText().trim();
    ModDet.VissibleInList = cvissibleinList.GetChecked();
    ModDet.OrderBy = cOrderBy.GetValue();
    ModDet.Mandatory = cMandetory.GetChecked();
    ModDet.Formula = cmemoFormula.GetText().trim();
    ModDet.AddEdit = $('#AddEdit').val();
    ModDet.Values = ctxtValues.GetText().trim();

    OtherDet.ModDet = ModDet;

    $.ajax({
        type: "POST",
        url: "ModuleDetails.aspx/AddField",
        data: JSON.stringify(OtherDet),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            jAlert(msg.d.split('~')[1]);
            if (msg.d.split('~')[0] == '0') {
                cGrid.Refresh();
                Cancel();
            }

        }
    });
}


function cchkIsFormulaChange() {
    if (cchkIsFormula.GetChecked()) {
        $('#formuladiv').slideDown();
        $('[data-toggle="popover"]').popover({ html: true });
        setPopoverContent();
        ccolumnList.PerformCallback();
    }
    else {
        $('#formuladiv').slideUp();
    }
}


function FormulaAdd() {

    cmemoFormula.SetText(cmemoFormula.GetText() + '[' + ccolumnList.GetText()+'] ')
}


function fieldTypeChange() {
    if (cFiledType.GetText() != "Formula") {
        $('#formuladiv').slideUp();
    } else {
        $('#formuladiv').slideDown();
        $('[data-toggle="popover"]').popover({ html: true });
        setPopoverContent();
        ccolumnList.PerformCallback();
    }



    if (cFiledType.GetText() != "Drop Down") {
        $('#commaSepearedtedDiv').slideUp();
    } else {
        $('#commaSepearedtedDiv').slideDown();
    }



   // cchkIsFormula.SetChecked(false);
  //  cchkIsFormulaChange();
    cmemoFormula.SetText('');
    ctxtValues.SetText('');
}


function PanelEndCallBack() {
    if (cFiledType.GetText() != "Formula") {
        //   $('#chkFormula').hide(); 
        $('#formuladiv').slideUp();
    } else {
        $('#chkFormula').show();
        $('#formuladiv').slideDown();
        $('[data-toggle="popover"]').popover({ html: true });
        setPopoverContent();
    }

    if (cFiledType.GetText() != "Drop Down") {
        $('#commaSepearedtedDiv').slideUp();
    } else {
        $('#commaSepearedtedDiv').slideDown();
    }


    cFiledType.SetEnabled(false);
    ctxtName.SetEnabled(false);
}


function onEditClick(id) {
    cComponentPanel.PerformCallback(id);
}


function Cancel() {
    
    cFiledType.SetEnabled(true);
    cFiledType.SetText('');
    fieldTypeChange();
    ctxtName.SetEnabled(true);
    ctxtName.SetText('');
    cOrderBy.SetValue(0);
    cvissibleinList.SetChecked(false);
    cMandetory.SetChecked(false);
    cchkIsFormula.SetChecked(false);
    cmemoFormula.SetText('');
    $('#AddEdit').val('Add');
}



function onDeleteClick(id) {
    deleetId = id;
    jConfirm('Delete?', "Alert", function (ret) {
        if (ret) {
            var OtherDet = {};
            OtherDet.id = deleetId;

            $.ajax({
                type: "POST",
                url: "ModuleDetails.aspx/DeleteField",
                data: JSON.stringify(OtherDet),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    jAlert(msg.d.split('~')[1]);
                    if (msg.d.split('~')[0] == '0') {
                        cGrid.Refresh();
                        //  Cancel();
                    }

                }
            });
        }
    });

   
}


function setPopoverContent() {
    $('#formulapopover').attr('data-content', "Select predefined fields and click on 'Add' to get the field in Formula Editor.<br />Now you can use operators like <strong>+,-,*,/,%,<,>,(,),<>,==</strong> to get desired calculated <br />value as output in the field.<br /><strong>Example:</strong><br />[Basic]+[DA]+[TA] to get the sum of the three(03) fields which are defined by you using Custom Module as 'Basic','DA','TA'.<br />So, you may use operators as per your choice in formula editor to get desired<br />calulated value.<br /><br /><strong><i>Fuctions and their Syntax:</i></strong><br /><br /><strong>if (expression)</strong> <br />An if-statement that contains an expression is differentiated from a traditional-if such as If FoundColor <> Blue by making the character after the word 'if' an open-parenthesis. Although this is usually accomplished by enclosing the entire expression in parentheses, it can also be done with something like if (x > 0) and (y > 0). In addition, the open-parenthesis may be omitted entirely if the first item after the word 'if' is a function call or an operator such as 'not' or '!'.<br /><br /><strong>toFixed(n):</strong><br />To fixed Decimal.");

}
 