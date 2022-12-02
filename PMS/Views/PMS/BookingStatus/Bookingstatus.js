
$(document).ready(function () {
    gridBookingStatusList.Refresh();

    $('#ddlAppIds').on('change', function () {
        if ($("#ddlAppIds option:selected").index() > 0) {
            var selectedValue = $(this).val();
            $('#ddlAppIds').prop("selectedIndex", 0);
            var url = '@Url.Action("ExportBookingStatuslist", "BookingStatus", new { type = "_type_" })'
            window.location.href = url.replace("_type_", selectedValue);
        }
    });
});
function gridRowclick(s, e) {
    $('#gridBookingStatusList').find('tr').removeClass('rowActive');
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
                $(value).css({ 'opacity': '1' });
            }, 100);
        });
    }, 200);
}
function OnStartCallback(s, e) {

}

var chkArr = "";

function OpenBookingforEdit(obj) {
    //alert(obj);
    $("#BOOKING_ID").val(obj);
    $.ajax({
        type: "POST",
        url: "@Url.Action("ViewDataShow", "BookingStatus")",
        data: { BOOKING_ID: obj },
    dataType: "json",
    success: function (response) {
        var status = response;
        var str = "";

        if (status != null) {
            $("#exampleModalLabel").html("Edit Booking Staus");
            $("#BOOKING_NAME").prop('disabled', true);
            $("#BOOKING_NAME").val(status.BOOKING_NAME);
            $("#DESCRIPTION").val(status.DESCRIPTION);
            $("#ddlType").val(status.BOOKING_TYPE);
            setPopulateStatusData(status.STATUS);
            $("#ddlBranch").val(status.BRANCH);
            setTimeout(function () {
                $("#ddlStatus").val(status.STATUS).change();
            }, 600);
            // $("#ddlStatus").val(status.STATUS);
            $("#txtColor").val(status.COLOR);
            $("#bookingSatus").modal('toggle');

            $("#btnSave").removeClass('hide');
        }

    },
    error: function (response) {
        //alert(response);
        jAlert("Please try again later.");
        //LoadingPanel.Hide();
    }
});
}

function OpenBookingforDelete(obj) {
    jConfirm('Confirm Delete?', 'Alert', function (r) {
        if (r) {
            $("#BOOKING_ID").val(obj);
            $.ajax({
                type: "POST",
                url: "@Url.Action("DeleteData", "BookingStatus")",
                data: { BOOKING_ID: obj },
            dataType: "json",
            success: function (response) {
                var status = response;
                jAlert(status);
                gridBookingStatusList.Refresh();
            },
            error: function (response) {
                //  alert(response);
                jAlert("Please try again later.");
            }
        });
    }
    else {
    // alert("false");
}
});
}


function OpenBookingStatusorView(obj) {
    //alert(obj);
    $("#BOOKING_ID").val(obj);
    $.ajax({
        type: "POST",
        url: "@Url.Action("ViewDataShow", "BookingStatus")",
        data: { BOOKING_ID: obj },
    dataType: "json",
    success: function (response) {
        var status = response;
        var str = "";

        if (status != null) {
            $("#exampleModalLabel").html("View Booking Staus");
            $("#BOOKING_NAME").prop('disabled', true);
            $("#BOOKING_NAME").val(status.BOOKING_NAME);
            $("#DESCRIPTION").val(status.DESCRIPTION);
            $("#ddlType").val(status.BOOKING_TYPE);
            setPopulateStatusData(status.STATUS);
            $("#ddlBranch").val(status.BRANCH);

            // $('#ddlStatus value=' + status.STATUS + '').attr('selected', 'true');
            $("#txtColor").val(status.COLOR);
            setTimeout(function () {
                $("#ddlStatus").val(status.STATUS).change();
            }, 600);
                   
            $("#btnSave").addClass('hide');
            $("#bookingSatus").modal('toggle');
        }

    },
    error: function (response) {
        // alert(response);
        jAlert("Please try again later.");
    }
});
}

function BookingSave() {
    LoadingPanel.Show();
    var obj = {};
    obj.BOOKING_ID = $("#BOOKING_ID").val();
    obj.BOOKING_NAME = $("#BOOKING_NAME").val().trim();
    obj.DESCRIPTION = $("#DESCRIPTION").val().trim();
    var e = document.getElementById("ddlType");
    obj.BOOKING_TYPE = e.options[e.selectedIndex].value;
    var f = document.getElementById("ddlBranch");
    obj.BRANCH = f.options[f.selectedIndex].value;
    var g = document.getElementById("ddlStatus");
    obj.STATUS = g.options[g.selectedIndex].value;
    obj.COLOR = $("#txtColor").val();
    if (obj.BOOKING_NAME != "") {
        if (obj.BOOKING_TYPE != "0") {
            if (obj.STATUS != "0") {
                if (obj.DESCRIPTION != "") {
                    if (obj.BRANCH != "") {
                        $.ajax({
                            type: "POST",
                            url: "@Url.Action("SaveData", "BookingStatus")",
                            data: { booking: obj },
                        success: function (response) {
                                   

                                   
                                    
                            jAlert(response, 'alert', function () {
                                LoadingPanel.Hide();
                                if (response == 'Saved Successfully.' || response == 'Update Successfully.') {
                                    $("#BOOKING_ID").val('');
                                    $("#BOOKING_NAME").val('');
                                    $("#DESCRIPTION").val('');
                                    $("#ddlType").val(0);
                                    $("#ddlBranch").val(0);
                                    $("#ddlStatus").val(0);
                                    $("#bookingSatus").modal('toggle');
                                    gridBookingStatusList.Refresh();
                                    gridBookingStatusList.Refresh();
                                }
                                else if (response == 'Name already exists.') {
                                    $("#BOOKING_NAME").focus();
                                }
                            });
                            //  $("#txtColor").val('');
                                       
                            // $("#btnSave").removeClass('hide');
                                        
                        },
                        error: function (response) {
                            //alert(response);
                            //jAlert("Please try again later");
                            jAlert("Please try again later.", "Alert", function () {
                                setTimeout(function () {
                                    $('#BOOKING_NAME').focus();
                                }, 200);
                            });
                            LoadingPanel.Hide();
                        }
                    });
                }
                else {
                    jAlert("Unit is Mandatory.", "Alert", function () {
                        setTimeout(function () {
                            $('#ddlBranch').focus();
                        }, 200);
                    });
                    LoadingPanel.Hide();
                }
            }
            else {
                jAlert("Description  is Mandatory.", "Alert", function () {
                    setTimeout(function () {
                        $('#DESCRIPTION').focus();
                    }, 200);
                });
                LoadingPanel.Hide();
            }
        }
        else {
            jAlert("Booking Status is Mandatory.", "Alert", function () {
                setTimeout(function () {
                    $('#ddlStatus').focus();
                }, 200);
            });
            LoadingPanel.Hide();
        }
    }
    else {
        jAlert("Type is Mandatory.", "Alert", function () {
            setTimeout(function () {
                $('#ddlType').focus();
            }, 200);
        });
        LoadingPanel.Hide();
    }
}
else {
            jAlert("Name is Mandatory.", "Alert", function () {
                setTimeout(function () {
                    $('#BOOKING_NAME').focus();
                }, 200);
            });
LoadingPanel.Hide();
}
}


function Close() {
    $("#BOOKING_ID").val('');
    $("#BOOKING_NAME").val('');
    $("#DESCRIPTION").val('');
    $("#ddlType").val(0);
    $("#ddlBranch").val(0);
    $("#ddlStatus").val(0);
    $("#txtColor").val('');
    $("#btnSave").removeClass('hide');
    PopulateStatusData();
    $("#exampleModalLabel").html("Add Booking Staus");
    $("#BOOKING_NAME").prop('disabled', false);
}

function PopulateStatusData() {
    $.ajax({
        type: "post",
        url: "@Url.Action("GetStatus", "BookingStatus")",
        data: { TypeId: $('#ddlType').val() },
    datatype: "json",
    traditional: true,
    success: function (data) {

        var status = "<select id='ddlStatus'>";
        status = status + '<option value="0">--Select--</option>';
        for (var i = 0; i < data.length; i++) {
            status = status + '<option value=' + data[i].STATUS_ID + '>' + data[i].STATUS_NAME + '</option>';
        }
        status = status + '</select>';
        $('#statusdiv').html(status);
    }
});
}

function setPopulateStatusData(values) {
    $.ajax({
        type: "post",
        url: "@Url.Action("GetStatus", "BookingStatus")",
        data: { TypeId: $('#ddlType').val() },
    datatype: "json",
    traditional: true,
    success: function (data) {

        var status = "<select id='ddlStatus'>";
        status = status + '<option value="0">--Select--</option>';
        for (var i = 0; i < data.length; i++) {
            status = status + '<option value=' + data[i].STATUS_ID + '>' + data[i].STATUS_NAME + '</option>';
        }
        status = status + '</select>';
        $('#statusdiv').html(status);
    }
});
$("#ddlStatus").val(values);
}

    $(document).ready(function () {
        if ($('body').hasClass('mini-navbar')) {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 90;
            gridBookingStatusList.SetWidth(cntWidth);
        } else {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 220;
            gridBookingStatusList.SetWidth(cntWidth);
        }

        $('.navbar-minimalize').click(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                gridBookingStatusList.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                gridBookingStatusList.SetWidth(cntWidth);
            }

        });
    });
