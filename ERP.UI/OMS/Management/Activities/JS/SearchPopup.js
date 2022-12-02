function callonServer(serviceurl, objectToPass, TargetID, HeaderCaption, UniqueIndex, onSelect) {
    $.ajax({
        
        type: "POST",
        url: serviceurl,
        data: JSON.stringify(objectToPass),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var myObj = msg.d;
            mycallonServerObj = msg.d;
            var txt = '';
            var count = 0;
            txt += "<table border='1' width='100%' class='dynamicPopupTbl'><tr class='HeaderStyle'> <th class='hide'>id</th>";// <th>Customer Name</th><th>Unique Id</th> <th>Address</th> </tr>"

            for (var i = 0 ; i < HeaderCaption.length; i++) {
                if (i == 0) {
                    txt += "<th class='" + UniqueIndex + "_" + i + "' style='width:300px' >" + HeaderCaption[i] + "</th>";
                } else {

                    txt += "<th class='" + UniqueIndex + "_" + i + "'>" + HeaderCaption[i] + "</th>";
                }
            }

           
            for (x in myObj) {
                txt += "<tr onclick='Rowclick(event," + onSelect + ")'>";
                var PropertyCount=0; 

                for (key in myObj[0]) {
                
                    if(PropertyCount==0)
                        txt += " <td class='hide' >" + myObj[x][key] + "</td>";
                    else if(PropertyCount==1)
                        txt += " <td title='" + myObj[x][key] + "'><input onclick='PopupTextClick(event," + onSelect + ")' type='text' style='background-color: #3399520a;'" + UniqueIndex + "='" + count + "'onfocus='searchElementGetFocus(event)'  onblur='searchElementlostFocus(event)' onkeydown=ValueSelected(event,'" + UniqueIndex.toString() + "') width='100%' readonly value='" + myObj[x][key] + "'/></td>";
                    else
                        txt +="<td>" + myObj[x][key] + "</td>"
                    PropertyCount++;
                }
                txt +="</tr>";
                count++;
            }
            txt += "</table>"
            document.getElementById(TargetID).innerHTML = txt;
        }
    });
}



function Rowclick(e, OnSelect) {
    if (e.target.type != "text") {
        var Id = e.target.parentElement.children[0].innerText;
        var name = e.target.parentElement.cells[1].children[0].value; 
        OnSelect.call(this, Id, name,e.target);
    }
}

function searchElementlostFocus(e) {
    e.target.parentElement.parentElement.className = "";
   // e.target.style = "background: transparent";
}


function searchElementGetFocus(e) {
    e.target.parentElement.parentElement.className = "focusrow";
   // e.target.style = "background: transparent";

}

function PopupTextClick(e, OnSelect) {
    var Id = e.target.parentElement.parentElement.children[0].innerText;
    var name = e.target.parentElement.parentElement.cells[1].children[0].value;
    OnSelect.call(this, Id, name,e.target.parentElement);
}