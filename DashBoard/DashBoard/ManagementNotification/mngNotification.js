
function reloadParent() {
    parent.document.location.href = '/oms/management/projectmainpage.aspx'
}
 
$(document).ready(function () {
    $('#detalsTable').hide();
    $('.shwDet').click(function (e) {
        $('#detalsTable').show()
    });
    getAllData();
});

function getAllData() {
    var dt = {};
    dt.action = "ALL";
    $.ajax({
        type: "POST",
        url: "mngNotification.aspx/GetAllNotificationData",
        data: JSON.stringify(dt),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {

            var data = data.d[0];
            console.log(data.TOTCUST);
            $('#totalCust').html(data.TOTCUST);
            $('#totalVend').html(data.TOTVEND);
            $('#totalEmp').html(data.TOTEMP);
            $('#totalInfl').html(data.CNTINF);
            $('#totalTrans').html(data.CNTTRANS);
        },
        error: function (data) {
            console.log(data);
        }
    });
}
function sendSms(number) {
    jAlert("SMS not Configured");
    console.log(number)
}
function sendMail() {
    //jAlert("Email not Configured");
    //console.log(number)
    var Body = CKEDITOR.instances['msgbody'].getData();
    if ($('#toEmail').val() == "") {
        jAlert('Please add an Email')
        return false;
    }
    if ($('#sub').val() == "") {
        jAlert('Please add Subjectline')
        return false;
    }
    if (Body == "") {
        jAlert('Please add a Message')
        return false;
    }
    var dt = {}
    dt.Email = $('#toEmail').val();
    dt.body = Body;
    dt.subject = $('#sub').val();
    //jAlert("Mail not Configured");
    $.ajax({
        type: "POST",
        url: "mngNotification.aspx/SendEmail",
        data: JSON.stringify(dt),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            var data = data.d;
            jAlert('Email Sent')
        },
        error: function (data) {
            console.log(data);
        }
    });
    //ekhane hobe
}
function getCustomer() {
    $('.itemType').removeClass('active');
    $('.one').addClass('active');
    var dt = {};
    dt.action = "Cust";
    $.ajax({
        type: "POST",
        url: "mngNotification.aspx/GetAllCustomer",
        data: JSON.stringify(dt),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            var data = data.d;
            console.log('data', data)
            var html = '';
            if (data.length != 0) {
                        
                for (i = 0; i < data.length; i++) {
                    var email = data[i].EMAIL;
                    html += '<tr><td>' + data[i].CONTACTPERSON + '</td>'
                    html += '<td>' + data[i].COMPANY + '</td>'
                    html += '<td>' + data[i].EVENT_TYPE + '</td>'
                    html += '<td><img src="../images/icons/smsPh.png" style="width:18px;cursor:pointer" data-number="' + data[i].PHNO + '" onclick="sendSms(' + data[i].PHNO + ')"></td>'
                    html += '<td><img src="../images/icons/email.png" style="width:18px;cursor:pointer" data-sub="' + data[i].EVENT_TYPE + '" data-email="' + data[i].EMAIL + '" class="openPopup"></td></tr>'
                }
                        
            } else {
                html += '<tr><td colspan="5" class="text-center">No Data to Display</td></tr>'
            }
            $('#datinTable').html(html);
        },
        error: function (data) {
            console.log(data);
        }
    });
}
function GetAllVendor() {
    $('.itemType').removeClass('active');
    $('.two').addClass('active');
    var dt = {};
    dt.action = "Vend";
    $.ajax({
        type: "POST",
        url: "mngNotification.aspx/GetAllVendor",
        data: JSON.stringify(dt),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            var data = data.d;
            var html = '';
            if (data.length != 0) {

                for (i = 0; i < data.length; i++) {
                    var email = data[i].EMAIL;
                    html += '<tr><td>' + data[i].CONTACTPERSON + '</td>'
                    html += '<td>' + data[i].COMPANY + '</td>'
                    html += '<td>' + data[i].EVENT_TYPE + '</td>'
                    html += '<td><img src="../images/icons/smsPh.png" style="width:18px;cursor:pointer" data-number="' + data[i].PHNO + '" onclick="sendSms(' + data[i].PHNO + ')"></td>'
                    html += '<td><img src="../images/icons/email.png" style="width:18px;cursor:pointer" data-sub="' + data[i].EVENT_TYPE + '" data-email="' + data[i].EMAIL + '" class="openPopup"></td></tr>'
                }

            } else {
                html += '<tr><td colspan="5" class="text-center">No Data to Display</td></tr>'
            }
            $('#datinTable').html(html);
        },
        error: function (data) {
            console.log(data);
        }
    });
}
function GetAllInfluencer() {
    $('.itemType').removeClass('active');
    $('.four').addClass('active');
    var dt = {};
    dt.action = "Influ";
    $.ajax({
        type: "POST",
        url: "mngNotification.aspx/GetAllInfluencer",
        data: JSON.stringify(dt),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            var data = data.d;
            console.log(data);
            var html = '';
            if (data.length != 0) {

                for (i = 0; i < data.length; i++) {
                    var email = data[i].EMAIL;
                    html += '<tr><td>' + data[i].CONTACTPERSON + '</td>'
                    html += '<td>' + data[i].COMPANY + '</td>'
                    html += '<td>' + data[i].EVENT_TYPE + '</td>'
                    html += '<td><img src="../images/icons/smsPh.png" style="width:18px;cursor:pointer" data-number="' + data[i].PHNO + '" onclick="sendSms(' + data[i].PHNO + ')"></td>'
                    html += '<td><img src="../images/icons/email.png" style="width:18px;cursor:pointer" data-sub="' + data[i].EVENT_TYPE + '" data-email="' + data[i].EMAIL + '" class="openPopup"></td></tr>'
                }
            } else {
                html += '<tr><td colspan="5" class="text-center">No Data to Display</td></tr>'
            }
            $('#datinTable').html(html);
        },
        error: function (data) {
            console.log(data);
        }
    });
}
function GetAllTransporter() {
    $('.itemType').removeClass('active');
    $('.five').addClass('active');
    var dt = {};
    dt.action = "Trans";
    $.ajax({
        type: "POST",
        url: "mngNotification.aspx/GetAllTransporter",
        data: JSON.stringify(dt),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            var data = data.d;
            console.log(data);
            var html = '';
            if (data.length != 0) {

                for (i = 0; i < data.length; i++) {
                    var email = data[i].EMAIL;
                    html += '<tr><td>' + data[i].CONTACTPERSON + '</td>'
                    html += '<td>' + data[i].COMPANY + '</td>'
                    html += '<td>' + data[i].EVENT_TYPE + '</td>'
                    html += '<td><img src="../images/icons/smsPh.png" style="width:18px;cursor:pointer" data-number="' + data[i].PHNO + '" onclick="sendSms(' + data[i].PHNO + ')"></td>'
                    html += '<td><img src="../images/icons/email.png" style="width:18px;cursor:pointer" data-sub="' + data[i].EVENT_TYPE + '" data-email="' + data[i].EMAIL + '" class="openPopup"></td></tr>'
                }

            } else {
                html += '<tr><td colspan="5" class="text-center">No Data to Display</td></tr>'
            }
            $('#datinTable').html(html);
        },
        error: function (data) {
            console.log(data);
        }
    });
}
function GetAllEmployee() {
    $('.itemType').removeClass('active');
    $('.three').addClass('active');
    var dt = {};
    dt.action = "Emp";
    $.ajax({
        type: "POST",
        url: "mngNotification.aspx/GetAllEmployee",
        data: JSON.stringify(dt),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            var data = data.d;
            console.log('Employee', data);
            var html = '';
            if (data.length != 0) {

                for (i = 0; i < data.length; i++) {
                    var email = data[i].EMAIL;
                    html += '<tr><td>' + data[i].COMPANY + '</td>'
                    html += '<td>' + data[i].CONTACTPERSON + '</td>'
                    html += '<td>' + data[i].EVENT_TYPE + '</td>'
                    html += '<td><img src="../images/icons/smsPh.png" style="width:18px;cursor:pointer" data-number="' + data[i].PHNO + '" onclick="sendSms(' + data[i].PHNO + ')"></td>'
                    html += '<td><img src="../images/icons/email.png" style="width:18px;cursor:pointer"  data-name="' + data[i].COMPANY + '" data-sub="' + data[i].EVENT_TYPE + '" data-email="' + data[i].EMAIL + '"  class="openPopup"></td></tr>'
                }

            } else {
                html += '<tr><td colspan="5" class="text-center">No Data to Display</td></tr>'
            }
            $('#datinTable').html(html);
        },
        error: function (data) {
            console.log(data);
        }
    });
}
        

$(document).ready(function () {
    $('body').on('click', '.openPopup', function () {
        $('#toEmail').val('');
        $('#sub').val('');
        $('#msgbody').val('');

        var emailId = $(this).data('email');
        var suject = $(this).data('sub');
        var name = $(this).data('name');
        setTimeout(function () {

            //console.log(emailId);
            $('#emailModal').modal('show');
            var emailid = emailId;
            $('#toEmail').val(emailid);
            $('#sub').val(suject);
            var str = "Hello " + name + " Wish you a very Happy " + suject + "";
            CKEDITOR.instances['msgbody'].setData(str);
        })
    })
});
$(document).ready(function () {
    var editor = CKEDITOR.instances['msgbody'];
    if (editor) { editor.destroy(true); }
    CKEDITOR.replace('msgbody', {
        height: '100px',
        uiColor: '#14B8C4',
        toolbar: [
            ['Bold', 'Italic', '-', 'NumberedList', 'BulletedList', '-', 'Link', 'Unlink'],
            ['FontSize', 'TextColor', 'BGColor'],
            ['Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak', 'Iframe']
        ],
    });
});