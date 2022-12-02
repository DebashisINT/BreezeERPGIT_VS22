function ReturnAftermsg() {
    jAlert("Saved Successfully.", "Alert", function () {
        location.href = 'AnnouncementList.aspx';
    });

}

function validate() {
    if ($("#hdnServicemanagement").val()=="1") {
        if (cisServiceManagement.GetChecked() == false) {
            if (cuserLookUp.GetText().trim() == "") {
                jAlert("You must select a user to proceed further.");
                return false;
            }
        }
    }
    else if ($("#hdnSTBManagementMasterSettings").val() == "1") {
        if (cisSTBManagement.GetChecked() == false) {
            if (cuserLookUp.GetText().trim() == "") {
                jAlert("You must select a user to proceed further.");
                return false;
            }
        }
    }
    else {
        if (cuserLookUp.GetText().trim() == "") {
            jAlert("You must select a user to proceed further.");
            return false;
        }
    }

    //    if ($("#hdnServicemanagement").val() == "0") {
    //    if (cuserLookUp.GetText().trim() == "") {
    //        jAlert("You must select a user to proceed further.");
    //        return false;
    //    }
    //}

    //if (cisSTBManagement.GetChecked() == false) {
    //    if (cuserLookUp.GetText().trim() == "") {
    //        jAlert("You must select a user to proceed further.");
    //        return false;
    //    }
    //}
    //else if ($("#hdnSTBManagementMasterSettings").val() == "0") {
    //    if (cuserLookUp.GetText().trim() == "") {
    //        jAlert("You must select a user to proceed further.");
    //        return false;
    //    }
    //}
   
    if (ctxtTitle.GetText().trim() == "") {
        jAlert("Title Cannot be blank.");
        return false;
    }
    
    //if (cancMemo.GetText().trim() == "") {
    //    jAlert("You must enter announcement text to proceed further.");
    //    return false;
    //}
    var htmltext = CKEDITOR.instances['ancMemo'].getData();
    if (htmltext != "" || htmltext != undefined) {
        $("#hdss").val(htmltext);
        var totextplain = htmltext.replace(/<[^>]+>/g, '');
        $("#hdssText").val(totextplain);
    }
    
   
    return true;
}

function selectAlluser() {
    cuserLookUp.gridView.SelectRows();
}

function unselectAlluser() {
    cuserLookUp.gridView.UnselectRows();
}