/*************************************************************************************************************************************
    Rev 1.0     Sanchita        V2.0.38     Message will be fired from first tab when logged out from the 2nd tab.Refer: 26091
 *************************************************************************************************************************************/

window.onunload = function () {
    eraseCookie('ERPACTIVEURL');
};

// Rev 1.0
document.addEventListener("visibilitychange", () => {
    // it could be either hidden or visible
    if (document.visibilityState === 'visible') {
        checkSessionLogoutMasterPage();
    }
});

function checkSessionLogoutMasterPage() {
    $.ajax({
        type: "POST",
        url: "/oms/Management/projectmainpage.aspx/checkSessionLogout",
        //data: JSON.stringify(dt),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.d == 1) {
                jAlert('Session expired !!!', 'Alert', function () {
                    window.parent.location.href = '/oms/login.aspx';
                });
            }
        },
        error: function (data) {
            console.log(data);
        }
    });
}
// End of Rev 1.0

(function (global) {

    if (typeof (global) === "undefined") {
        throw new Error("window is undefined");
    }

    var _hash = "!";
    var noBackPlease = function () {
        global.location.href += "#";

        // making sure we have the fruit available for juice....
        // 50 milliseconds for just once do not cost much (^__^)
        global.setTimeout(function () {
            global.location.href += "!";
        }, 50);
    };
    // Earlier we had setInerval here....
    global.onhashchange = function () {
        if (global.location.hash !== _hash) {
            global.location.hash = _hash;
        }
    };

    global.onload = function () {
        noBackPlease();

        // disables backspace on page except on input fields and textarea..
        document.body.onkeydown = function (e) {
            var elm = e.target.nodeName.toLowerCase();
            if (e.which === 8 && (elm !== 'input' && elm !== 'textarea')) {
                e.preventDefault();
            }
            // stopping event bubbling up the DOM tree..
            e.stopPropagation();
        };
    };

})(window);






function DecimalRoundoff(value, decimals) {
    return Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
}

function addCookiesKeyOnStorage(val) {
    var data = '';
    if (localStorage.getItem("GridCookiesId") != null) {
        data = localStorage.getItem("GridCookiesId");
    }
    var tempCookiesData = data + ',';
    if (tempCookiesData.indexOf(',' + val + ',') == -1) {
        data = data + ',' + val;
        localStorage.setItem("GridCookiesId", data);
    }


}

function showpage(obj) {

    OnSegmentChange(obj);
}

function AvailableExportOption() {
    var str = $('#drdExport').find('option:selected').text();
    if (str == 'Export to') {
        return false;
    }
    return true;
}

function OnSegmentChange(keyValue) {

    popup11.Show();
    var url = '../management/frm_selectCompFinYrSett.aspx?id=' + keyValue;
    //alert(url);
    //editwin = dhtmlmodal.open("Editbox", "iframe", url, "", "width=400px,height=245px,center=1,resize=0,scrolling=2,top=500", "recal")
    // document.getElementById('ctl00_ContentPlaceHolder1_Headermain1_cmbSegment').style.visibility = 'hidden';
    //editwin.onclose = function () {
    //    document.getElementById('ctl00_ContentPlaceHolder1_Headermain1_cmbSegment').style.visibility = 'visible';
    //    window.location = '/management/welcome.aspx';
    //}
}


function HideShow(obj, HS) {
    if (HS == "H")
        document.getElementById(obj).style.display = "None";
    else
        document.getElementById(obj).style.display = "inline";
}
function GetObjectID(obj) {
    return document.getElementById(obj);
}
FieldName = '';
window.history.forward();
function noBack() { window.history.forward(); }

function AddFavourite() {
    document.getElementById('BtnFavourite').click();
}

function OnMoreInfoClick(url, HeaderText, Width, Height, anyKey) //AnyKey will help to call back event to child page, if u have to fire more that one function 
{
    editwin = dhtmlmodal.open("Editbox", "iframe", url, HeaderText, "width=" + Width + ",height=" + Height + ",center=1,resize=1,scrolling=2,top=500", "recal")
    editwin.onclose = function () {
        if (anyKey == 'Y') {
            //document.getElementById('IFRAME_ForAllPages').contentWindow.callback();
        }
    }
}



function fn_AllowonlyNumeric(s, e) {
    var theEvent = e.htmlEvent || window.event;
    var key = theEvent.keyCode || theEvent.which;
    var keychar = String.fromCharCode(key);
    if (key == 9 || key == 37 || key == 38 || key == 39 || key == 40 || key == 8 || key == 46) { // Left / Up / Right / Down Arrow, Backspace, Delete keys
        return;
    }
    var regex = /[0-9\b]/;

    if (!regex.test(keychar)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault)
            theEvent.preventDefault();
    }
}


window.history.forward();
function noBack() {
    window.history.forward();
}


$(function () {
    var MainBodyClass = $('body').attr('class');

    var $navMainDiv = $('body .dpvsl-top');

    if (MainBodyClass == 'mini-navbar') {
        $.each($navMainDiv, function (index, value) {
            //$(this).hide();
            $(this).fadeOut("slow");
        });

    }
    $('.thrdlevel').mCustomScrollbar({
        theme: "rounded-dots",
        scrollInertia: 0
    });

});

$(document).ready(function () {
    //$("#btnShow").click(function ()
    //{alert('mmm')});
    document.cookie = "ERPACTIVEURL=1";
    $('.navbar-minimalize').click(function () {

        var bodyclass = $('body').attr('class');

        var $div = $('body .dpvsl-top');

        if (bodyclass == 'mini-navbar') {
            createCookie("MenuCloseOpen", "1", 30);
            $.each($div, function (index, value) {
                $(this).fadeIn('slow');
            });

        }
        else {
            eraseCookie('MenuCloseOpen');
            //alert();
            //$(".fa-angle-right dpvsl-top").hide();

            $.each($div, function (index, value) {
                $(this).fadeOut('slow');
            });
        }

    });

});
function eraseCookie(name) {
    createCookie(name, "", -1);
}

function SetActiveLink(pathName) {
    $('.sidenav').find('ul li').find('a').removeClass('active');

    var $a = $('.sidenav').find('ul li').find('a');

    $.each($a, function (index, value) {
        if ($(this).attr('href') == pathName) {
            $(this).addClass('active');
            $(this).parentsUntil('.sidenav').map(function () {
                if ($(this).children('a').length > 0) {
                    $.each($(this).children('a'), function (i, v) {
                        $(this).addClass('active');
                    });
                }
            });
        }
    });
}

function createCookie(name, value, days) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else var expires = "";
    document.cookie = name + "=" + value + expires + "; path=/";
}

function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

function DashboardClick()
{
    window.open('/DashBoard/MainDasBoard.aspx', '_blank');
}


function openMenuFromNav(menuUrl) {
    location.href = menuUrl;
}
