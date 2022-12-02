
var isFromCheckBox = 0;
var arrMultiPopup = new Array();
var isFormChechedFocus = 0;

function callonServerM(serviceurl, objectToPass, TargetID, HeaderCaption, UniqueIndex, onSelect, ArrName) {
    debugger;
    $.ajax({

        type: "POST",
        url: serviceurl,
        data: JSON.stringify(objectToPass),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var myObj = msg.d;
            var txt = '';
            var count = 0;


            txt += "<table border='1' width='100%' class='dynamicPopupTbl'><tr class='HeaderStyle'> <th >Select</th> <th class='hide'>id</th>";// <th>Customer Name</th><th>Unique Id</th> <th>Address</th> </tr>"
            for (var i = 0 ; i < HeaderCaption.length; i++) {
                txt += "<th>" + HeaderCaption[i] + "</th>";
            }


            for (x in myObj) {

                txt += "<tr onclick=RowclickMulti(event," + onSelect + ",ch" + ArrName + "" + count + ",'" + ArrName.toString() + "') >";

                var PropertyCount = 0;

                var arr = $.grep(arrMultiPopup, function (element) { return element.Name == ArrName })[0].ArraySource;
                var selectedarr = $.grep(arr, function (element) { return element.Id == myObj[x].id; });

                if (selectedarr != '') { // Wheather checked or not
                    txt += " <td><input  type='checkbox' " + UniqueIndex + "='" + count + "' checked='checked' id ='ch" + ArrName + "" + count + "' onchange=PopupCheckBoxClick(event," + onSelect + ",ch" + ArrName + "" + count + ",'" + ArrName.toString() + "')  onfocusin='searchElementGetFocus(event)'  onblur='searchElementlostFocus(event)' onkeydown=ValueSelectedMulti(event,'" + UniqueIndex.toString() + "',ch" + ArrName + "" + count + ",'" + ArrName.toString() + "') style='background-color: #3399520a;' /></td>";
                }
                else {
                    txt += " <td><input  type='checkbox' " + UniqueIndex + "='" + count + "' id ='ch" + ArrName + "" + count + "' onchange=PopupCheckBoxClick(event," + onSelect + ",ch" + ArrName + "" + count + ",'" + ArrName.toString() + "')  onfocusin='searchElementGetFocus(event)'  onblur='searchElementlostFocus(event)' onkeydown=ValueSelectedMulti(event,'" + UniqueIndex.toString() + "',ch" + ArrName + "" + count + ",'" + ArrName.toString() + "') style='background-color: #3399520a;' /></td>";
                }
                for (key in myObj[0]) {
                    if (PropertyCount == 0)
                        txt += " <td class='hide'>" + myObj[x][key] + "</td>";
                    else
                        txt += "<td>" + myObj[x][key] + "</td>"
                    PropertyCount++;
                }
                txt += "</tr>";
                count++;
            }
            txt += "</table>"
            document.getElementById(TargetID).innerHTML = txt;
        }
    });
}


function RowclickMulti(e, OnSelect, chk, ArrName) {
    isFromCheckBox = 0;
    if (e.target.type != "text" && e.target.type != "checkbox") {
        var Id = e.target.parentElement.children[1].innerText;
        var name = e.target.parentElement.children[2].innerText;
        var hsn = e.target.parentElement.children[4].innerText;

        if (chk.checked == true) {
            chk.checked = false;
        }
        else {
            chk.checked = true;
        }
        PopupMultiSelectCollection(Id, name, chk, ArrName, hsn);
    }

}

function searchElementlostFocus(e) {
    e.target.parentElement.parentElement.className = "";
}

function searchElementGetFocus(e) {
    e.target.parentElement.parentElement.className = "focusrow";
}


function PopupCheckBoxClick(e, OnSelect, chk, ArrName) {
    var Id = e.target.parentElement.parentElement.children[1].innerText;
    var name = e.target.parentElement.parentElement.children[2].innerText;
    var hsn = e.target.parentElement.parentElement.children[4].innerText;
    isFromCheckBox = 1;
    PopupMultiSelectCollection(Id, name, chk, ArrName,hsn);
}

function ValueSelectedMulti(e, indexName, chk, ArrName) {
    isFromCheckBox = 0;
    if (e.code == "Enter" || e.code == "NumpadEnter" || e.which == 32) {  // e.which == 32 (SpaceBar)
        // var Id = e.target.parentElement.parentElement.cells[1].innerText;
        // var name = e.target.parentElement.parentElement.cells[2].innerText;


        // PopupMultiSelectCollection(Id, name, chk);
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
            SetfocusOnseach('dPropertyIndex');
        }
    }
}

function PopupMultiSelectCollection(Id, name, chk, ArrName, hsn) {
    var Multiselect = new Object()
    if (chk.checked == true) {
        Multiselect.chkId = chk;
        Multiselect.Id = Id;
        Multiselect.Name = name;
        Multiselect.hsn = hsn;

        var arr = $.grep(arrMultiPopup, function (element) { return element.Name == ArrName })[0].ArraySource;
        var selectedarr = $.grep(arr, function (element) { return element.Id == Id; });
        if (selectedarr == '') {
            $.grep(arrMultiPopup, function (element) { return element.Name == ArrName })[0].ArraySource.push(Multiselect);
        }
    }
    else {
        $.grep(arrMultiPopup, function (element) { return element.Name == ArrName })[0].ArraySource = $.grep(arrMultiPopup, function (element) { return element.Name == ArrName })[0].ArraySource.filter(function (item) {
            return item.Id !== Id;
        });
    }

}

function OKPopup(ArrName) {

    var AddMultiIds = '';
    var AddMultiNames = '';
    var MultiIds = '';
    var MultiNames = '';
    MultiHSNCRP = '';

    for (var i = 0; i < $.grep(arrMultiPopup, function (element) { return element.Name == ArrName })[0].ArraySource.length; i++) {
        MultiIds = MultiIds + ',' + $.grep(arrMultiPopup, function (element) { return element.Name == ArrName })[0].ArraySource[i].Id;
        MultiNames = MultiNames + ',' + $.grep(arrMultiPopup, function (element) { return element.Name == ArrName })[0].ArraySource[i].Name;
        MultiHSNCRP = MultiHSNCRP + ',' + $.grep(arrMultiPopup, function (element) { return element.Name == ArrName })[0].ArraySource[i].hsn;
    }

    if (arrMultiPopup.length == 0) {
        MultiIds = '';
        MultiNames = '';
        AddMultiIds = '';
        AddMultiNames = '';
    }
    else {
        AddMultiIds = MultiIds.substr(1);
        AddMultiNames = MultiNames.substr(1);
    }

    SetSelectedValues(AddMultiIds, AddMultiNames, ArrName);
}

function DeSelectAll(ArrName) {
    var testarr = new Array();
    testarr = $.grep(arrMultiPopup, function (element) { return element.Name == ArrName })[0].ArraySource;
    for (var k = 0 ; k < testarr.length; k++) {
        testarr[k].chkId.checked = false;

        $.grep(arrMultiPopup, function (element) { return element.Name == ArrName })[0].ArraySource = $.grep(arrMultiPopup, function (element) { return element.Name == ArrName })[0].ArraySource.filter(function (item) {
            return item.Id !== testarr[k].Id;
        });

    }

    MultiIds = '';
    MultiNames = '';
    AddMultiIds = '';
    AddMultiNames = '';
    SetSelectedValues(AddMultiIds, AddMultiNames);
}