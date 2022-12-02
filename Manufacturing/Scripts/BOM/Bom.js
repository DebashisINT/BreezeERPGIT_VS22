$.noConflict();
var globalRowIndex;
$(document).ready(function () {
    
    setTimeout(function () { AddNewRowWithSl(); }, 200);
    $('#NumberingModel').on('shown.bs.modal', function () {
        $('#txtNumberingSearch').focus();
    })


    //LoadNumbering Schem
    $.post("/Mfbom/GetNumberingScheme", { SearchKey: $('#txtNumberingSearch').val() }, function (result) {
        listOfNumeringScheme = JSON.parse(result);
    });

});




function AddNewRowWithSl() {
    grid.AddNewRow(); 
    resuffleSerial(); 
}
function resuffleSerial() {
    var sl = 1;
    for (var i = -1; i > -500; i--) {
        if (grid.GetRow(i)) {
            grid.batchEditApi.StartEdit(i, 1);
            grid.GetEditor('Sl').SetText(sl);
            if (grid.GetEditor('Product').GetText() == "") { 
                grid.GetEditor('Quantity').SetText(0); 
                grid.GetEditor('Conversion').SetText(0); 
                grid.GetEditor('QtyIssue').SetText(0); 
                grid.GetEditor('Price').SetText(0); 
                grid.GetEditor('Amount').SetText(0);
            }
            grid.batchEditApi.StartEdit(i, 1);
            sl = sl + 1;
        }
    }
}



function btnFinishedProduct_ButtonClick(s, e) {

}

function btnwarehouseStockIn_ButtonClick(s, e) {

}

function productSelection_ButtonClick(s, e) {

}

function btnNumberingScheem_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        btnNumberingScheem_ButtonClick(s, e);
    }
}

function grid_CustomButtonClick(s, e) {
    if (e.buttonID == "Delete") {
        grid.DeleteRow(e.visibleIndex);
        resuffleSerial();
    }
    e.processOnServer = false;
}

function remarksKeyDown(s, e) {
    if (e.htmlEvent.key == 'Enter') {
        AddNewRowWithSl();
    }
}

function BatchStartEditing(s, e) {
    globalRowIndex = e.visibleIndex;
}

function QuantityValidate(s, e) {
    if(s.GetValue()>10)
        e.isValid = false;
    else
        e.isValid = true;
}



//Numbering Scheme
function txtNumberingSearchkeydown(e) {
    //if (e.code == "Enter" || e.code == "NumpadEnter") {
    //    if ($('#txtNumberingSearch').val().trim() != '') {
           
    //    }
    //}
    if (e.code == "ArrowDown") {
        if ($("input[NumberingScheme=0]"))
            $("input[NumberingScheme=0]").focus();
        $("input[NumberingScheme=0]").parent('td').addClass('focusrow');
    } else {
        filtertable($('#txtNumberingSearch').val());
    }
}

function filtertable(searchKey) {
    var SchemeObj;
    if (searchKey == "")
        SchemeObj = listOfNumeringScheme;
    else
        SchemeObj = $.grep(listOfNumeringScheme, function (e) { return e.SchemaName.toUpperCase().indexOf(searchKey.toUpperCase()) != -1; })

    var html = "<table border='1' width='100%' class='dynamicPopupTbl'>";
    html = html + ' <tr class="HeaderStyle">  <th>Name</th>  </tr>';
    for (var i = 0; i < SchemeObj.length ; i++) {
        html = html + '<tr><td><input type="text" readonly onClick=clickNum('+SchemeObj[i].Id+') onkeydown=numInput(' + SchemeObj[i].Id + ',' + i + ',event)  NumberingScheme=' + i + ' width="100%" value="' + SchemeObj[i].SchemaName + '"/> </td></tr>';
    }
    html = html + "</table>"
    console.log(html);
    $('#ProductTable').html(html);
}

function btnNumberingScheem_ButtonClick(s, e) {
    $('#NumberingModel').modal('show');

    $('#txtNumberingSearch').val('');
    var html = "<table border='1' width='100%' class='dynamicPopupTbl'>";
    html = html + ' <tr class="HeaderStyle">  <th>Name</th>  </tr>';
    for (var i = 0; i < listOfNumeringScheme.length ; i++) {
        html = html + '<tr><td><input type="text" readonly onClick=clickNum(' + listOfNumeringScheme[i].Id + ') onkeydown=numInput(' + listOfNumeringScheme[i].Id + ',' + i + ',event)  NumberingScheme=' + i + ' width="100%" value="' + listOfNumeringScheme[i].SchemaName + '"/> </td></tr>';
    }
    html = html + "</table>"
    console.log(html);
    $('#ProductTable').html(html);
}

function numInput(id, index, e) {
    $("input[NumberingScheme]").parent().removeClass('focusrow');
    if (e.code == "Enter" || e.code == "NumpadEnter") { 
        $('#NumberingModel').modal('hide');
        var selectedNumberingScheme = $.grep(listOfNumeringScheme, function (e) { return e.Id == id; })
        populateDocNo(selectedNumberingScheme[0]);
    }
    else if (e.code == "ArrowDown") {
        $("input[NumberingScheme=" + (index + 1) + "]").focus();
        $("input[NumberingScheme=" + (index + 1) + "]").parent('td').addClass('focusrow');
    }
    else if (e.code == "ArrowUp") {
        if (index == 0) {
            $('#txtNumberingSearch').focus();
        } else {
            $("input[NumberingScheme=" + (index - 1) + "]").focus();
            $("input[NumberingScheme=" + (index - 1) + "]").parent('td').addClass('focusrow');
        }
    }

}

function populateDocNo(obj) {
    if (obj.startno == 1) {

        txtDocNo.SetText(obj.prefix + "XXXXXXXXXXXXXXXXX".substr(0, obj.digit) + obj.suffix);
        txtDocNo.SetEnabled(false);
        dtDocDate.Focus();
    }
    else {
        txtDocNo.SetEnabled(true);
        txtDocNo.SetText('');
        txtDocNo.Focus();
    }
    btnNumberingScheem.SetText(obj.SchemaName);
}

function clickNum(id) {
    $('#NumberingModel').modal('hide');
    var selectedNumberingScheme = $.grep(listOfNumeringScheme, function (e) { return e.Id == id; })
    populateDocNo(selectedNumberingScheme[0]);
}