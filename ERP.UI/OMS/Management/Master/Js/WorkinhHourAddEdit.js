function SavedSuccessfull() {
    jAlert("Saved Successfull.", "Alert", function () {
        window.location.href = 'frm_workingShedule.aspx';
    });
}
function ErrorShow(msg) {
    jAlert(msg);
}

function Validate(s, e) {
    if (ctxtName.GetText().trim() == "") {
        jAlert("Please provide Roster Name.");
        e.processOnServer = false;
        return false;
    }


    if (cchkMonday.GetChecked()) {
        if (cbeginMonday.GetValue() == null){
            jAlert("Please provide Begin time for Monday.");
            e.processOnServer = false;
            return false;
        } else if (cEndmonday.GetValue() == null) {
            jAlert("Please provide End time for Monday.");
            e.processOnServer = false;
            return false;
        }
        
    }
    
    if (cchktuesday.GetChecked()) {
        if (cBeginTuesDay.GetValue() == null) {
            jAlert("Please provide Begin time for Tuesday.");
            e.processOnServer = false;
            return false;
        } else if (cendTuesDay.GetValue() == null) {
            jAlert("Please provide End time for Tuesday.");
            e.processOnServer = false;
            return false;
        }

    }

    if (cchkWednesday.GetChecked()) {
        if (cbeginWednesday.GetValue() == null) {
            jAlert("Please provide Begin time for Wednesday.");
            e.processOnServer = false;
            return false;
        } else if (cendWednesday.GetValue() == null) {
            jAlert("Please provide End time for Wednesday.");
            e.processOnServer = false;
            return false;
        }
    }

    if (cchkThursday.GetChecked()) {
        if (cbeginThursday.GetValue() == null) {
            jAlert("Please provide Begin time for Thursday.");
            e.processOnServer = false;
            return false;
        } else if (cendThursday.GetValue() == null) {
            jAlert("Please provide End time for Thursday.");
            e.processOnServer = false;
            return false;
        }
    }

    if (cchkFriday.GetChecked()) {
        if (cbeginFriday.GetValue() == null) {
            jAlert("Please provide Begin time for Friday.");
            e.processOnServer = false;
            return false;
        } else if (cendFriday.GetValue() == null) {
            jAlert("Please provide End time for Friday.");
            e.processOnServer = false;
            return false;
        }
    }

    if (cchkSaturday.GetChecked()) {
        if (cbeginSaturday.GetValue() == null) {
            jAlert("Please provide Begin time for Saturday.");
            e.processOnServer = false;
            return false;
        } else if (cendSaturday.GetValue() == null) {
            jAlert("Please provide End time for Saturday.");
            e.processOnServer = false;
            return false;
        }
    }

    if (cchkSunday.GetChecked()) {
        if (cbeginSunday.GetValue() == null) {
            jAlert("Please provide Begin time for Sunday.");
            e.processOnServer = false;
            return false;
        } else if (cendSaturday.GetValue() == null) {
            jAlert("cendSunday provide End time for Sunday.");
            e.processOnServer = false;
            return false;
        }
    }

}

 
function mondayChkChange() {
    //if (!cchkMonday.GetChecked()) {
    //    $('.tdMonday').hide();
    //    cbeginMonday.SetValue(null);
    //    cEndmonday.SetValue(null);
    //    cbrkMonday.SetValue(null);

    //} else {
    //    $('.tdMonday').show();
    //}
}

function tuesdayCheckChange() {
    //if (!cchktuesday.GetChecked()) {
    //    $('.tdTuesday').hide();
    //    cBeginTuesDay.SetValue(null);
    //    cendTuesDay.SetValue(null);
    //    cbrkTuesDay.SetValue(null);

    //} else {
    //    $('.tdTuesday').show();
    //}
}

function WednesdayCheckChange() {
    //if (!cchkWednesday.GetChecked()) {
    //    $('.tdWednesday').hide();
    //    cbeginWednesday.SetValue(null);
    //    cendWednesday.SetValue(null);
    //    cbrkWednesday.SetValue(null);
    //} else {
    //    $('.tdWednesday').show();
    //} 
}

function thursdayCheckChange() { 
    //if (!cchkThursday.GetChecked()) { 
    //    $('.tdthursday').hide();
    //    cbeginThursday.SetValue(null);
    //    cendThursday.SetValue(null);
    //    cbrkThursday.SetValue(null);

    //} else { 
    //    $('.tdthursday').show(); 
    //}
}

function friDayCheckChange() {
    //if (!cchkFriday.GetChecked()) {
    //    $('.tdFriday').hide();
    //    cbeginFriday.SetValue(null);
    //    cendFriday.SetValue(null);
    //    cbrkFriday.SetValue(null);
    //} else {
    //    $('.tdFriday').show();
    //} 
}

function SaturdayCheckChange() {
    //if (!cchkSaturday.GetChecked()) {
    //    $('.tdSaturday').hide();
    //    cbeginSaturday.SetValue(null);
    //    cendSaturday.SetValue(null);
    //    cbrkSaturday.SetValue(null);

    //} else {
    //    $('.tdSaturday').show();

    //}
}

function sundayCheckChange() { 
    //if (!cchkSunday.GetChecked()) {
    //    $('.tdSunday').hide();
    //    cbeginSunday.SetValue(null);
    //    cendSunday.SetValue(null);
    //    cbrkSunday.SetValue(null);
    //} else {
    //    $('.tdSunday').show();
    //} 
}


 