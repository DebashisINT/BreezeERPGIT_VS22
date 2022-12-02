
var isFromCheckBox = 0;
var arrMultiPopup = new Array();
var isFormChechedFocus = 0;

function callonServer(serviceurl, objectToPass, TargetID, HeaderCaption, UniqueIndex, onSelect) {
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

                txt += "<tr onclick='Rowclick(event," + onSelect + ", ch" + count + " )'>";
                var PropertyCount = 0;

                if ($.grep(arrMultiPopup, function (element) { return element.Id == myObj[x].id; }) != '') { // Wheather checked or not
                   txt += " <td><input  type='checkbox' " + UniqueIndex + "='" + count + "' checked='checked' id ='ch" + count + "' onchange='PopupCheckBoxClick(event," + onSelect + ",ch" + count + ")'  onfocusin='searchElementGetFocus(event)'  onblur='searchElementlostFocus(event)' onkeydown=ValueSelected(event,'" + UniqueIndex.toString() + "',ch" + count + ") style='background-color: #3399520a;' /></td>";
               }
               else {
                   txt += " <td><input  type='checkbox' " + UniqueIndex + "='" + count + "' id ='ch" + count + "' onclick='PopupCheckBoxClick(event," + onSelect + ",ch" + count + ")'  onfocusin='searchElementGetFocus(event)'  onblur='searchElementlostFocus(event)' onkeydown=ValueSelected(event,'" + UniqueIndex.toString() + "',ch" + count + ") style='background-color: #3399520a;' /></td>";
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


function Rowclick(e, OnSelect, chk) {
    isFromCheckBox = 0;
    if (e.target.type != "text" && e.target.type != "checkbox") {
        var Id = e.target.parentElement.children[1].innerText;
        var name = e.target.parentElement.children[2].innerText;

        if (chk.checked == true) {
            chk.checked = false;
        }
        else {
            chk.checked = true;
        }
        //PopupMultiSelectCollection(Id, name, chk.checked);
        PopupMultiSelectCollection(Id, name, chk);
    }

}

function searchElementlostFocus(e) {
         e.target.parentElement.parentElement.className = "";
}

function searchElementGetFocus(e) {
       e.target.parentElement.parentElement.className = "focusrow";
}


function PopupCheckBoxClick(e, OnSelect, chk) {
    var Id = e.target.parentElement.parentElement.children[1].innerText;
    var name = e.target.parentElement.parentElement.children[2].innerText;
    isFromCheckBox = 1;
    PopupMultiSelectCollection(Id, name, chk);
}

function ValueSelected(e, indexName, chk) {
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

function PopupMultiSelectCollection(Id, name, chk) {
    var Multiselect = new Object()
    if (chk.checked == true) {
        Multiselect.Id = Id;
        Multiselect.Name = name;
        //if ($.grep(arrMultiPopup, function (element) { return element.Id == Id; }) == '') {
        arrMultiPopup.push(Multiselect);
         //}
    }
    else {
        arrMultiPopup.pop($.grep(arrMultiPopup, function (element) { return element.Id == Id; }));
    }

}

function OKPopup() {

    var AddMultiIds = '';
    var AddMultiNames = '';
    var MultiIds = '';
    var MultiNames = '';
    for (var i = 0; i < arrMultiPopup.length; i++) {
        MultiIds = MultiIds + ',' + arrMultiPopup[i].Id;
        MultiNames = MultiNames + ',' + arrMultiPopup[i].Name;
    }

    if (arrMultiPopup.length == 0)
    {
        MultiIds = '';
        MultiNames = '';
        AddMultiIds = '';
        AddMultiNames = '';
    }
    else {
        AddMultiIds = MultiIds.substr(1);
        AddMultiNames = MultiNames.substr(1);
    }
    
    SetSelectedValues(AddMultiIds, AddMultiNames);
}
