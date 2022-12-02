
function storeControlDiv(id) {
    $('#widthBlock').show();
    $('#plusid').show();
    $('#minusid').show();
    

    lastFocusedId = id;
    $('div').removeClass('controlGetFocus')
    var focusedDiv = document.getElementById('div' + id);
    focusedDiv.className = focusedDiv.className + ' controlGetFocus';
}

function plusClick() {
    var newclassName = GetNextColmd(document.getElementById('div' + lastFocusedId).className.replace('controlGetFocus', ''));
    document.getElementById('div' + lastFocusedId).className = newclassName + " controlGetFocus"
}


function minusClick() {

    var newclassName = GetPrevColmd(document.getElementById('div' + lastFocusedId).className.replace('controlGetFocus', ''));
    document.getElementById('div' + lastFocusedId).className = newclassName + " controlGetFocus"
}

function GetNextColmd(now) {
    now = now.trim();
    if (now == "col-md-1")
        now = "col-md-2";
    else if (now == "col-md-2")
        now = "col-md-3";
    else if (now == "col-md-3")
        now = "col-md-4";
    else if (now == "col-md-4")
        now = "col-md-5";
    else if (now == "col-md-5")
        now = "col-md-6";
    else if (now == "col-md-6")
        now = "col-md-7";
    else if (now == "col-md-7")
        now = "col-md-8";
    else if (now == "col-md-8")
        now = "col-md-9";
    else if (now == "col-md-9")
        now = "col-md-10";
    else if (now == "col-md-10")
        now = "col-md-11";
    else if (now == "col-md-11")
        now = "col-md-12";
    return now;
}

function GetPrevColmd(now) {
    now = now.trim();
    if (now == "col-md-12")
        now = "col-md-11";
    else if (now == "col-md-11")
        now = "col-md-10";
    else if (now == "col-md-10")
        now = "col-md-9";
    else if (now == "col-md-9")
        now = "col-md-8";
    else if (now == "col-md-8")
        now = "col-md-7";
    else if (now == "col-md-7")
        now = "col-md-6";
    else if (now == "col-md-6")
        now = "col-md-5";
    else if (now == "col-md-5")
        now = "col-md-4";
    else if (now == "col-md-4")
        now = "col-md-3";
    else if (now == "col-md-3")
        now = "col-md-2";
    else if (now == "col-md-2")
        now = "col-md-1";

    return now;
}




function allowDrop(ev) {
    ev.preventDefault();
}
function drop(ev) {
    ev.preventDefault();

    var data = ev.dataTransfer.getData("text");
    if (!data) {
        var newNode = document.createElement('div');
        newNode.innerHTML = "<span onclick=removeDiv(event) style='left: 50%; position: absolute;'><i class='fa fa-times-circle'></i></span>";
        newNode.className = "clear newlineClas";
        if (ev.target.draggable)
            ev.target.after(newNode);
        else
            ev.target.appendChild(newNode);
    }
    else {
        if (ev.target.className.indexOf("clear") == 0) {
            if (ev.target.draggable)
                ev.target.parentNode.after(document.getElementById(data));
            else
            ev.target.parentNode.appendChild(document.getElementById(data));
        }
        else {
            if (ev.target.draggable)
                ev.target.after(document.getElementById(data));
            else
            ev.target.appendChild(document.getElementById(data));
        }
    }
    initiateDoubleClick();
}

function drag(ev) { 
   ev.dataTransfer.setData("text", "");
}


function removeDiv(e) {
    e.currentTarget.parentNode.remove();
}


function dragControl(ev) {
    ev.dataTransfer.setData("text", ev.target.id);
}



function saveData() {
    var lsitOfData = [];
    var childrenCount = $('#divdropid').children().length;
    for (var i = 0 ; i < childrenCount; i++) {
        var obj = {};



        if ($('#divdropid').children()[i].tagName == 'SPAN') {


            if ($('#divdropid').children()[i].children[0].draggable) {
                obj.id = $('#divdropid').children()[i].children[0].id.replace('div', '');
                obj.className = $('#divdropid').children()[i].children[0].className.replace('controlGetFocus', '').trim();
                obj.DisplayName = $('#' + $('#divdropid').children()[i].children[0].id + ' label')[0].innerHTML;
            } else {
                obj.className = $('#divdropid').children()[i].children[0].className.trim();
            }



        } else {
            if ($('#divdropid').children()[i].draggable) {
                obj.id = $('#divdropid').children()[i].id.replace('div', '');
                obj.className = $('#divdropid').children()[i].className.replace('controlGetFocus', '').trim();
                obj.DisplayName = $('#' + $('#divdropid').children()[i].id + ' label')[0].innerHTML;
            } else {
                obj.className = $('#divdropid').children()[i].className.trim();
            }
        }
        lsitOfData.push(obj);

    }

    console.log(lsitOfData);

    var OtherDet = {};
    OtherDet.formDesigner = lsitOfData;
    OtherDet.MainId = $('#hdMainId').val();

    $.ajax({
        type: "POST",
        url: "UserFormDesign.aspx/UpdateDesgin",
        data: JSON.stringify(OtherDet),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            jAlert(msg.d.split('~')[1]);
            if (msg.d.split('~')[0] == '0') {
              
            }

        }
    });

}

var GlobalTextField;
function initiateDoubleClick() {
    $('#divdropid label').dblclick(function () {
        GlobalTextField = this;
        $('#txtRename').val(this.innerHTML);
        $('#RenameModel').modal('show');
        document.getElementById('TestMyinp').style.color = "";
        TestMyinput();
    });

}


function updateDisplayText() {
    if ($('#txtRename').val().trim() == "") {
        jAlert("You must enter Display text to proceed further.")
        return;
    }
    GlobalTextField.innerHTML = $('#txtRename').val();
    $('#txtRename').val('');
    $('#RenameModel').modal('hide');
}


function TestMyinput() {
    $('#TestMyinp').html($('#txtRename').val());
}



function colorchange() {
    //$('#copiedbox').val($('#idcolor').val());
   // document.getElementById("copiedbox").value = document.getElementById('idcolor').value;
    //var copyText = document.getElementById("copiedbox");
    //copyText.select();
    //setTimeout(function () { document.execCommand("copy"); }, 500);

  

    document.getElementById('TestMyinp').style.color = document.getElementById('idcolor').value;
    var actualOuterHtml = document.getElementById('TestMyinp').outerHTML.replace('<label', '<span');
    actualOuterHtml = actualOuterHtml.replace('</label>', '</span>');
    $('#txtRename').val(actualOuterHtml.replace('margin-left: 25px;', '').replace('id="TestMyinp"', ''));
    //$('#txtRename').val(document.getElementById('TestMyinp').outerHTML.replace('margin-left: 25px;', '').replace('id="TestMyinp"', ''));
}

function getSelectionHtml() {
    var html = "";
    if (typeof window.getSelection != "undefined") {
        var sel = window.getSelection();
        if (sel.rangeCount) {
            var container = document.createElement("div");
            for (var i = 0, len = sel.rangeCount; i < len; ++i) {
                container.appendChild(sel.getRangeAt(i).cloneContents());
            }
            html = container.innerHTML;
        }
    } else if (typeof document.selection != "undefined") {
        if (document.selection.type == "Text") {
            html = document.selection.createRange().htmlText;
        }
    }
    return html;
}