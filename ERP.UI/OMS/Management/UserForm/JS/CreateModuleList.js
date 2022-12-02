function OnAddNewClick() {
    cAddNew.Show();
}

function closePopup() {
    cAddNew.Hide();
}

function onEditClick(id) {
    window.location.href = 'ModuleDetails.aspx?id=' + id;

}

function SaveNew() {

    if ($('#txtModName').val().trim() == "") {
        jAlert("You must enter Module name to proceed further.")
        return;
    }
    var format = /[!~@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/;
    if (format.test($('#txtModName').val().trim())) {
        jAlert("Name cannot contain any special character.")
        return;
    }



    var OtherDetails = {}
    OtherDetails.ModName = $('#txtModName').val();
    $.ajax({
        type: "POST",
        url: "CreateModuleList.aspx/CreateModule",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            jAlert(msg.d.split('~')[1]);
            if (msg.d.split('~')[0] == '0') {
                $('#txtModName').val('');
                cAddNew.Hide();
                cGrid.Refresh();
            }

        }
    });
}


function onMenu(id) {

    var OtherDetails = {}
    OtherDetails.ModName = cGrid.GetRow(id).children[0].innerText;
    $.ajax({
        type: "POST",
        url: "CreateModuleList.aspx/CreateMenu",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            jAlert(msg.d.split('~')[1]);

        }
    });
}



function onUserwise(id) {
    var OtherDetails = {}
    OtherDetails.id = id;
    $.ajax({
        type: "POST",
        url: "CreateModuleList.aspx/MakeUserwise",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            jAlert(msg.d.split('~')[1]);
            cGrid.Refresh();
        }
    });

}


function DocDesign(id) {
    window.open('CustomDesginer.aspx?id='+id, '_blank')
}


function ModDesign(id) {
    window.location.href = 'UserFormDesign.aspx?id=' + id;
}



function ModSettings(id) {
    $('#hdModId').val(id);
    SettingsPopup.Show();
    cFiledType.PerformCallback(id);
}

function SaveDateFilter() {

    var OtherDetails = {}
    OtherDetails.fieldName = cFiledType.GetValue();
    OtherDetails.hdModId = $('#hdModId').val();
    $.ajax({
        type: "POST",
        url: "CreateModuleList.aspx/UpdateDateFilter",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            jAlert(msg.d.split('~')[1]);


            if (msg.d.split('~')[0] != "-1") {
                SettingsPopup.Hide();
            }
        }
    });
}


function CloseUserPopup() {
    cUserpopup.Hide();
}

function ShowModUser(modId) {
    $('#hdModId').val(modId);
    cuserGrid.PerformCallback('BindGrid~' + modId);
    cUserpopup.Show();
}

function SaveUserModuleWise() {
    cuserGrid.PerformCallback('Save');
}
function Saveandcloseuserpopup() {
    jAlert('Mapped Successfully.')
    cUserpopup.Hide();
}

function userGridEndCallBack() {
    if (cuserGrid.cpRetMsg && cuserGrid.cpRetMsg == "1")
    {
        Saveandcloseuserpopup();
    }
}