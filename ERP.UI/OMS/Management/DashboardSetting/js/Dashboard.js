$(document).ready(function () {
    $('#UserGroupModel').on('shown.bs.modal', function () {
        clearPopup();
        $('#txtEmpSearch').focus();
    })

    $('#btnAdd').click(
                  function (e) {
                      $('#list1 > option:selected').appendTo('#list2');
                      e.preventDefault();
                  });

    $('#btnAddAll').click(
    function (e) {
        $('#list1 > option').appendTo('#list2');
        e.preventDefault();
    });

    $('#btnRemove').click(
    function (e) {
        $('#list2 > option:selected').appendTo('#list1');
        e.preventDefault();
    });

    $('#btnRemoveAll').click(
    function (e) {
        $('#list2 > option').appendTo('#list1');
        e.preventDefault();
    });

    var jsonobj = JSON.parse($("#jsonlistdiv").text());
    var AccordianHTML = '';
    var ParentObj = $.grep(jsonobj, function (e) { return e.parent_id == "0" })


    for (var pid = 0; pid < ParentObj.length; pid++) {

        AccordianHTML =AccordianHTML+ '<div class="panel panel-default"><div class="panel-heading"><h4 class="panel-title">';
        AccordianHTML = AccordianHTML + '<a data-toggle="collapse" data-parent="#DynamicAccordian" class="collapsed" href="#collapse' + ParentObj[pid].id + '">' + ParentObj[pid].text + '</a>';
        AccordianHTML = AccordianHTML + '<input type="checkbox" onchange=change_check('+ParentObj[pid].id+') id="ch' + ParentObj[pid].id + '"/></h4></div>';

        var ChildObj = $.grep(jsonobj, function (e) { return e.parent_id == ParentObj[pid].id });


        AccordianHTML = AccordianHTML + '<div id="collapse' + ParentObj[pid].id + '" class="panel-collapse collapse"><div class="panel-body"><ul class="inline-list">';
        for (var cid = 0; cid < ChildObj.length ; cid++) {
            AccordianHTML = AccordianHTML + '<li><input type="checkbox" id="chk' + ChildObj[cid].id + '" />' + ChildObj[cid].text + '</li>'
        }
        AccordianHTML = AccordianHTML + '</ul></div></div></div>'


    }
    $('#DynamicAccordian').html(AccordianHTML);

    if ($("#Hidden_add_edit").val() == "edit") {
        rightsChecking();
    }


})


function change_check(id)
{
    
    var jsonobj = JSON.parse($("#jsonlistdiv").text());
    var ParentObj1 = $.grep(jsonobj, function (e) { return e.parent_id == id })
    for (var jid = 0; jid < ParentObj1.length; jid++) {
        document.getElementById('chk' + ParentObj1[jid].id).checked = document.getElementById('ch'+id).checked;
    }



    //for (var jid = 0; jid < ParentObj1.length; jid++)
    //{
    //    if(document.getElementById('ch'+id).checked)
    //    {
    //        document.getElementById('chk' + ParentObj1[jid].id).checked = true;
    //    }
    //    else
    //    {
    //        document.getElementById('chk' + ParentObj1[jid].id).checked = false;
    //    }
    //}

   
    
}

function rightsChecking() {


    var jsonobj = JSON.parse($("#jsonlistdiv").text());
    var jsonobj1 = JSON.parse($("#jsonlisteditdiv").text());


    for (var pid = 0; pid < jsonobj1.length; pid++) {
        var ParentObj = $.grep(jsonobj, function (e) { return e.column_name == jsonobj1[pid].column_name });
        document.getElementById('chk' + ParentObj[0].id).checked = jsonobj1[pid].status;
    }




    //var jsonobj = JSON.parse($("#jsonlistdiv").text());
    //var ParentObj = $.grep(jsonobj, function (e) { return e.parent_id != "0" })
    //var jsonobj1 = JSON.parse($("#jsonlisteditdiv").text());
    //for (var pid = 0; pid < jsonobj1.length; pid++) {
    //    if(jsonobj1[pid].column_name==ParentObj[pid].column_name)
    //    {
    //        document.getElementById('chk' + ParentObj[pid].id).checked = jsonobj1[pid].status;
    //    }

    //}



}

function UserGroupSelect() {
    $('#UserGroupModel').modal('show');
}
function UserGroupKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        UserGroupSelect();
    }
}



function LoadUserGroup(e) {
    var _groupdetails = {};
    _groupdetails._searchkey = $("#txtEmpSearch").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("User Group Name");
        if ($("#txtEmpSearch").val() != "") {
            callonServer("Service/DashBoard.asmx/GetUser", _groupdetails, "UserTable", HeaderCaption, "UserIndex", "SetUser");
            e.preventDefault();
            return false;
        }

    }
    else if (e.code == "ArrowDown") {
        if ($("input[UserIndex=0]"))
            $("input[UserIndex=0]").focus();
    }
}

function SetUser(id, Name) {
    $("#UserGroupModel").modal('hide');
    cUserButtonEdit.SetText(Name);
    $('#UserId').val(id);
    if (id != '' || id != null) {
        BindListBoxUser(id);
    }


}

function BindListBoxUser(id) {
    $.ajax({
        type: "POST",
        url: "Service/DashBoard.asmx/GetAllUser",
        data: JSON.stringify({ 'id': id }),
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            // console.log(response);
            if (response.d) {
                if (response.d.msg == "true") {
                    
                    var luser = $('select[id$=list1]');
                    luser.empty();
                    var listItems = [];

                    var ddlArray= new Array(); 
                    var ddl = document.getElementById('list2'); 
                    for (i = 0; i < ddl.options.length; i++) {
                        ddlArray[i] = ddl .options[i].value;
                    }

                    $.each(response.d._usergroup, function (index, e) {
                        if (ddlArray.indexOf(e.id) == -1) {
                            listItems.push("<option value=" + e.id + "> " + e.name + "</option>");
                        }
                        
                    });


                    $(luser).append(listItems.join(''));
                }
                else {
                    alert(response.d.msg);

                }

            }

        },
        error: function (response) {

            console.log(response);
        }
    });



}

function ValueSelected(e, indexName) {
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var Id = e.target.parentElement.parentElement.cells[0].innerText;
        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
        if (Id) {
            SetUser(Id, name);
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

            $('#txtEmpSearch').focus();
        }
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

function apply() {
    if (ctxt_setting_nm.GetText() == "") {
        $("#setting_nm").show();
        return
    }
    else {
        $("#acnt_nm").hide();
    }

    if ($('#list2 option').length > 0)

    {
        var _header = {
            user_group_name: ctxt_setting_nm.GetText(),
            user_grp_id: $('#UserId').val(),
            action: $('#Hidden_add_edit').val(),
            dashboard_id: $('#Hidn_dash_board_id').val()

        }

        var rights_dtls = [];

        var jsonobj = JSON.parse($("#jsonlistdiv").text());
        var ParentObj = $.grep(jsonobj, function (e) { return e.parent_id != "0" })
        //console.log(ParentObj);
        for (var pid = 0; pid < ParentObj.length; pid++) {
            var rights = {};

            rights['column_name'] = ParentObj[pid].column_name;
            rights['status'] = document.getElementById('chk' + ParentObj[pid].id).checked;
            rights_dtls.push(rights);

        }
        var listItems = [];
        var listBox = document.getElementById("list2");

        for (var i = 0; i < listBox.options.length; i++) {
            var Items = {};

            Items['value'] = listBox.options[i].value;
            Items['text'] = listBox.options[i].innerHTML;
            listItems.push(Items);
        }

        var apply = {
            header: _header,
            users_dtls: listItems,
            rights_dtls: rights_dtls
        }

        $.ajax({
            type: "POST",
            url: "Service/DashBoard.asmx/save",
            data: "{apply:" + JSON.stringify(apply) + "}",//JSON.stringify(apply),
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                console.log(response);
                if (response.d) {
                    if (response.d == "true") {
                        jAlert("Saved Successfully", "Alert", function () {
                            window.location.href = 'DashBoardSettingList.aspx';
                        });
                    }
                    else {
                        jAlert(response.d);

                    }

                }

            },
            error: function (response) {

                console.log(response);
            }

        });
    }
    else
    {
        jAlert("Select Atleast One User!");
        return;
    }
    
}
function cancel() {
    location.href = "DashBoardSettingList.aspx"
}
