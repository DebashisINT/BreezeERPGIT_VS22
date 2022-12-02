function cmbParentId_Change() {

}
function ClearPopUp() {
    $("#cmbGroup_I").val('');
    ccmbGroupType.ClearItems();
    ctxtAccountName.SetValue('');
    ctxtAccountShortName.SetValue('');
    ctxtGroupSequence.SetValue('');
    ctxtGroupSchedule.SetValue('');
    ccmbParentId.ClearItems();
}
function CloseWindow() {
    cAddEditPopUp.Hide();
}


function AddAccountType() {
    ccmbGroupType.PerformCallback();
    ccmbParentId.PerformCallback();
}
function ShowHideFilter(obj) {
    grid.PerformCallback(obj);
}
function AddNewRow() {
    $("#CallStatus").val('Add');
    $("#GroupId").val("");
    ClearPopUp();
    ctxtAccountShortName.SetEnabled(true);
    //$("#txtAccountShortName_I").prop("disabled", false);
    //$("#txtAccountName_I").prop("disabled", false);

    cAddEditPopUp.Show();
}
function EndCall(obj) {

    if (grid.cpDelmsg != null)
        jAlert(grid.cpDelmsg);
}
function OnCountryChanged(cmbParent) {
    grid.GetEditor("ParentIDWithName").PerformCallback(cmbParent.GetValue().toString());
}
function PerformCallToGridBind() {
    if (ccmbGroup.GetText() == "") {
        valid = false;
        jAlert('Please select valid group type to proceed.', 'Error');
        return;
    }
    if (ctxtGroupSequence.GetText() == "" || ctxtGroupSequence.GetText() == "0") {
        valid = false;
        jAlert('Please select valid group sequence to proceed.', 'Error');
        return;
    }
    if (ctxtAccountShortName.GetText() == "") {
        valid = false;
        jAlert('Please select valid short name to proceed.', 'Error');
        return;
    }
    if (ctxtAccountName.GetText() == "") {
        valid = false;
        jAlert('Please select valid  name to proceed.', 'Error');
        return;
    }
    var status = $("#CallStatus").val();
    var id = $("#GroupId").val();
    cSelectPanel.PerformCallback("Save~" + status + "~" + id);
}
function EditForm(id) {
    var GroupId = id;
    $("#GroupId").val(id);
    ClearPopUp();

    $("#CallStatus").val("Edit");
    cAddEditPopUp.Show();

    cSelectPanel.PerformCallback('Edit~' + GroupId);
    //$("#txtAccountShortName").prop("disabled", true);
    //$("#txtAccountName").prop("disabled", true);
}
function SelectPanel_EndCallBack() {
    if (cSelectPanel.cpAutoID != "") {
        if (cSelectPanel.cpAutoID == "Edit") {
            cAddEditPopUp.Hide();
            ClearPopUp();
            jAlert("Data Updated Successfully");
            grid.Refresh();
        }
        else if (cSelectPanel.cpAutoID == "Delete") {
            jAlert("Data Deleted Successfully");
            grid.Refresh();
        }
        else if (cSelectPanel.cpAutoID == "Add") {
            jAlert("Data Saved Successfully!");
            cAddEditPopUp.Hide();
            ClearPopUp();
            grid.Refresh();
        }
        else {
            jAlert("Try agian Later");
        }
    }
}
function TypeChange() {


}
function OnClickDelete(id) {
    var GroupId = id;
    $("#GroupId").val(id);
    ClearPopUp();
    $("#CallStatus").val("Delete");
    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
        if (r == true) {
            cSelectPanel.PerformCallback('Delete~' + GroupId);
        }
    });

} 
function gridRowclick(s, e) {
    $('#AccountGroup').find('tr').removeClass('rowActive');
    $('.floatedBtnArea').removeClass('insideGrid');
    //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
    $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
    $(s.GetRow(e.visibleIndex)).addClass('rowActive');
    setTimeout(function () {
        //alert('delay');
        var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
        //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
        //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
        //    setTimeout(function () {
        //        $(this).fadeIn();
        //    }, 100);
        //});    
        $.each(lists, function (index, value) {
            //console.log(index);
            //console.log(value);
            setTimeout(function () {
                console.log(value);
                $(value).css({ 'opacity': '1' });
            }, 100);
        });
    }, 200);
}
    $(document).ready(function () {
        if ($('body').hasClass('mini-navbar')) {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 90;
            grid.SetWidth(cntWidth);
        } else {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 220;
            grid.SetWidth(cntWidth);
        }
        $('.navbar-minimalize').click(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                grid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                grid.SetWidth(cntWidth);
            }
        });
    });
