

function validatedate(dateid) {


    // this is use for you can type only numeric
    $(dateid).on('keypress', function (e) {
        return e.metaKey || // cmd/ctrl
          e.which <= 0 || // arrow keys
          e.which == 8 || // delete key
          /[0-9]/.test(String.fromCharCode(e.which)); // numbers
    })
    // end 

    $(dateid).focusout(function () {

        var dateformat = /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/;
        var Val_date = $(dateid).val();
        if (Val_date == '') {

        } else {

            if (Val_date.match(dateformat)) {
                var seperator = Val_date.split('-');
                if (seperator.length > 1) {
                    var splitdate = Val_date.split('-');
                }
                var dd = parseInt(splitdate[0]);
                var mm = parseInt(splitdate[1]);
                var yy = parseInt(splitdate[2]);
                var ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
                if (mm == 1 || mm > 2) {
                    if (dd > ListofDays[mm - 1]) {
                        jAlert('Invalid date format!');
                        return false;
                    }
                }
                if (mm == 2) {
                    var lyear = false;
                    if ((!(yy % 4) && yy % 100) || !(yy % 400)) {
                        lyear = true;
                    }
                    if ((lyear == false) && (dd >= 29)) {
                        jAlert('Invalid date format!');
                        return false;
                    }
                    if ((lyear == true) && (dd > 29)) {
                        jAlert('Invalid date format!');
                        return false;
                    }
                }
            }
            else {
                jAlert("Invalid date format!");
                return false;
            }
        }
    });

}

function dateValidationFormat(e) {
    debugger;
    var v = e.target.value; //this.value;
    if (v.match(/^\d{2}$/) !== null) {
        e.target.value = v + '-';
    } else if (v.match(/^\d{2}\-\d{2}$/) !== null) {
        e.target.value = v + '-';
    }
}

