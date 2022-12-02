
var globalindexcheck = 0;
var cpSelectedKeys = [];
function EntitySelectionChanged(s, e) {
    if (e.isChangedOnServer) return;
    globalindexcheck = e.visibleIndex;
    var key = s.GetRowKey(e.visibleIndex);
    if (e.isSelected) {
        cpSelectedKeys.push(key);
    }
    else {
        cpSelectedKeys = RemoveElementFromArray(cpSelectedKeys, key);
        
    }
    appcode = cpSelectedKeys;

    AddDataToSelectedList(cpSelectedKeys);

    
}

function AddDataToSelectedList(cpSelectedKeys) {
    $.ajax({
        type: "POST",
        url: "../CRMMembers/AddDataToSelectedList",
        data: JSON.stringify(cpSelectedKeys),

        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
    }
});
}

function RemoveElementFromArray(array, element) {
    var index = array.indexOf(element);
    if (index < 0) return array;
    array[index] = null;
    var result = [];
    for (var i = 0; i < array.length; i++) {
        if (array[i] === null)
            continue;
        result.push(array[i]);
    }
    return result;
}

function gvEntityBeginCallback(s, e) {
    e.customArgs["EntityType"] = $("input[name='Entity_type']:checked").val();;
}

function crmMemberTypeChange() {
    gvEntity.Refresh();
    gvEntity.Refresh();

}

function SaveCRMMembers() {
    //var abc = $('#Module_Name').val();
    var obj = {};
    obj.Module_Name = $('#Module_Name').val();
    obj.Module_id = $('#hdnCampaign_Id').val();

    $.ajax({
        type: "POST",
        url: "../CRMMembers/SaveMembers",
        data: JSON.stringify(obj),
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            jAlert(response);
            if (response != "Please select atleast one entity to proceed.") {
                CRMpcControl.Hide();
            }
        }
    });
}