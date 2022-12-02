var textSeparator = ",";




function updateText() {
    var selectedItems = checkListBox.GetSelectedItems();
    checkComboBox.SetText(getSelectedItemsText(selectedItems));
}
function synchronizeListBoxValues(dropDown, args) {
    checkListBox.UnselectAll();
    var texts = dropDown.GetText().split(textSeparator);
    var values = getValuesByTexts(texts);
    checkListBox.SelectValues(values);
    updateText(); // for remove non-existing texts
}
function getSelectedItemsText(items) {
    var texts = [];
    for (var i = 0; i < items.length; i++)
        texts.push(items[i].text);
    return texts.join(textSeparator);
}
function getValuesByTexts(texts) {
    var actualValues = [];
    var item;
    for (var i = 0; i < texts.length; i++) {
        item = checkListBox.FindItemByText(texts[i]);
        if (item != null)
            actualValues.push(item.value);
    }
    return actualValues;
}


function clickOnSave() {
    if (cmemoRemarks.GetText() == "") {
        jAlert("Remarks Cannot be blank.");
        return;
    }
    if (ccmbOpenClose.GetValue() == "O" && cdtNextFollowupdate.GetValue() == null) {
        jAlert("You must select Next Followup date to proceed further.");
        return;
    }
    if (ccmbOpenClose.GetValue() == "D" && cdtNextFollowupdate.GetValue() == null) {
        jAlert("You must select Next Followup date to proceed further.");
        return;
    }
    cComponentPanel.PerformCallback('Save');
}


function PanelEndCallback() {
    if (cComponentPanel.cpRetMsg) {
        if (cComponentPanel.cpRetMsg != '') {
            jAlert(cComponentPanel.cpRetMsg);

            if(cComponentPanel.cpRetMsg =='Document Deleted Successfully.')
                cGridDetail.Refresh();
            cComponentPanel.cpRetMsg = null;
        }
    }
    if (cComponentPanel.cpisSuccess == "True") {
        clearAll();
        cGridDetail.Refresh();
        cComponentPanel.cpisSuccess = null;
    }
    onStatusChange();
}


function clearAll() {
    $('#hdAction').val('Add');
    checkComboBox.SetText('');
    ccmbFollowUp.SetValue('Phone');
    ccmbOpenClose.SetValue('O');
    cdtNextFollowupdate.SetValue(null);
    cmemoRemarks.SetText('');
    cdtNextFollowupdate.SetEnabled(true);
    cdtFollowDate.SetDate(new Date());
    if($('#hdNoDays').val() !="0")
    cdtNextFollowupdate.SetMaxDate(addDays(cdtFollowDate.GetDate(), parseFloat($('#hdNoDays').val())));
}


function addDays(date, days) {
    var result = new Date(date);
    result.setDate(result.getDate() + days);
    return result;
}

function OnView(key) {
    cdtNextFollowupdate.SetEnabled(false);
    $('#hdFollowId').val(key);
    cComponentPanel.PerformCallback('Edit~' + key);
}




$(document).ready(function () {

    if ($('#hdlastStatus').val() != "") { 
        $("#alertId").html("System found that your most of the follow-up are done using " + $('#hdlastStatus').val() + ". Would you like to proceed with " + $('#hdlastStatus').val() + "? <a href=# onclick=setUsing('" + $('#hdlastStatus').val() + "')> Click here to Select " + $('#hdlastStatus').val() + " </a>");
        $("#alertdiv").removeClass('hide');

        setTimeout(function () { document.getElementById('closeAlert').click(); }, 15000);
    }
    else {
        if ($('#hdStatusNewCust').val() != "") { 
            $("#alertId").html("For the selected Customer, would you like to use " + $('#hdStatusNewCust').val() + " as follow-up?  <a href=# onclick=setUsing('" + $('#hdStatusNewCust').val() + "')>  Click here to Select " + $('#hdStatusNewCust').val() + " </a>");
            $("#alertdiv").removeClass('hide');
            setTimeout(function () { document.getElementById('closeAlert').click(); }, 15000);
        }
    }

    setTimeout(function () { cdtNextFollowupdate.SetMaxDate(addDays(cdtFollowDate.GetDate(), parseFloat($('#hdNoDays').val()))); }, 1500);

});


function setUsing(setUsing) {
    ccmbFollowUp.SetValue(setUsing);
    //$("#alertdiv").addClass('hide');
    document.getElementById('closeAlert').click();
}


function onStatusChange() {
    //if (ccmbOpenClose.GetValue() == "C") {
    //    cdtNextFollowupdate.SetValue(null);
    //    cdtNextFollowupdate.SetEnabled(false);
    //} else {
    //    cdtNextFollowupdate.SetEnabled(true);
    //}
}


var DeleteID;
function onDelete(key) {
    DeleteID = key;
    jConfirm("Confirm Delete?", "Delete", function (ret) {
        if (ret) {
            cComponentPanel.PerformCallback('Delete~' + DeleteID);
        }
    });

}



function SelectAllDocument() {

    var doclist = '';
    for (var i = 0; i < checkListBox.GetItemCount() ; i++) {
        doclist = doclist + ',' + checkListBox.GetItem(i).text;
    }
    doclist = doclist.substr(1, doclist.length);
    checkListBox.SelectAll();
    checkComboBox.SetText(doclist);
}