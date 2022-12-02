
window.onunload = function () {
    eraseCookie('ERPACTIVEURL');
};

var ModuleNameForHelp = "";


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
    //$('.thrdlevel').mCustomScrollbar({
    //    theme: "rounded-dots",
    //    scrollInertia: 0
    //});

});

$(document).ready(function () {


    if (ModuleNameForHelp != "") {
        $('#btnHelpPopup').show();
    }
    


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



function ShowHelpPopup() {
    var OtherDetails = {}
    OtherDetails.ModName = ModuleNameForHelp;
    $.ajax({
        type: "POST",
        url: "/OMS/MasterPage/Services/Master.asmx/GetHelpHtmlByModName",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            console.log(msg);
            $('#helptextpart').html(msg.d);
        }
    });



    var newNode = document.createElement('div');
    newNode.className = "helpDivClassoverlay";
    document.body.appendChild(newNode);

    $('#HelpDivid').show();
}


function CloseHelpPopup() {
    $('.helpDivClassoverlay').remove();
    $('#HelpDivid').hide();
}
//Added after separation

$(document).ready(function () {
    var WindowHeight = $(window).height();
    var SetmenuHeight = WindowHeight - 120;

    $('.fixedMenuNew .sidenav > ul').css({ 'max-height': SetmenuHeight });
    //$('.mini-navbar .fixedMenuNew').mouseover(function () {
    //    setTimeout(function () {
    //        $('.fixedMenuNew .sidebar .navfltext').css({ 'width': 'auto', 'opacity': '1', 'display': 'inline-block' })
    //    }, 1000);

    //});
            
});
$(window).resize(function () {
    var WindowHeight = $(window).height();
    var SetmenuHeight = WindowHeight - 120;
    $('.fixedMenuNew .sidenav > ul').css({ 'max-height': SetmenuHeight });
})
$(function(){

            

});
        
// end start
$(document).ready(function () {
        function getTime() {
            var d = new Date();
            d.setHours(d.getHours()); // offset from local time
            var h = (d.getHours() % 12) || 12; // show midnight & noon as 12
            return (
                (h < 10 ? '0' : '') + h +
                (d.getMinutes() < 10 ? ':0' : ':') + d.getMinutes() +
                        // optional seconds display
                // ( d.getSeconds() < 10 ? ':0' : ':') + d.getSeconds() + 
                (d.getHours() < 12 ? ' AM' : ' PM')
            );

        }

        var clock = document.getElementById('clock');
        setInterval(function () { clock.innerHTML = getTime(); }, 1000);


        function formatDate(date) {
            var monthNames = [
              "January", "February", "March",
              "April", "May", "June", "July",
              "August", "September", "October",
              "November", "December"
            ];

            var day = date.getDate();
            var monthIndex = date.getMonth();
            var year = date.getFullYear();

            return day + ' ' + monthNames[monthIndex] + ' ' + year;
        }
        var dayDate = document.getElementById('dayDate');
        setInterval(function () { dayDate.innerHTML = formatDate(new Date()); }, 1000);
    });
//end start  





    $(document).ready(function () {
        $('.uptodate').hide()
            

        $('.showUpdatepop').click(function (e) {
            e.preventDefault()
            $('#updateModal').modal('show');
            var obj = {};
            obj.links = '/version/details';
            $.ajax({
                type: "POST",
                url: "/oms/Management/projectmainpage.aspx/getversiondetails",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(obj),
                dataType: "json",
                success: function (data) {
                    console.log('version.aspx', data)
                    var data = data.d;
                    if (data.length > 0) {
                        var localdata = data[0];
                        $('.uptodate').hide();
                        $('.updatePanel').show();
                        //localStorage.setItem('version_name', data[0].version_name)
                        versions = data;
                        $('.latestVersion').text(localdata.version_name);
                        $('.whatsNewIn').text(localdata.whatsnew);
                        $('.versionDate').text(localdata.version_date);
                    } else {
                        $('.uptodate').show()
                        $('.updatePanel').hide()
                    }
                }
            });
        });
        //$('#updateApp').click(function (e) {
        //    e.preventDefault()
        //    $.confirm({
        //        title: 'Warning!',
        //        content: 'Are you sure you want to Install',
        //        buttons: {
        //            approve: {
        //                text: 'Yes Install',
        //                btnClass: 'btn-success',
        //                keys: ['enter', 'shift'],
        //                action: function () {
        //                    var localStore = JSON.parse(localStorage.getItem('versionData'));

        //                    var vrObj = {};
        //                    vrObj.links = '/Version/Save';
        //                    vrObj.requestedversion = localStore.version_name;
        //                    $.ajax({
        //                        type: "POST",
        //                        url: "ProjectMainPage.aspx/SaveVersionDetails",
        //                        contentType: "application/json; charset=utf-8",
        //                        data: JSON.stringify(vrObj),
        //                        dataType: "json",
        //                        success: function (data) {
        //                            $.alert({
        //                                title: 'Message',
        //                                content: data.d,
        //                            });
        //                        }
        //                    });
        //                }
        //            },
        //            reject: {
        //                text: 'No Next time may be',
        //                btnClass: 'btn-danger',
        //                keys: ['enter', 'shift'],
        //                action: function () {

        //                }
        //            }
        //        }
        //    });
        //});


        $('.showupdatepop').click(function (e) {
            e.preventdefault()
            $('#updatemodal').modal('show');
        });
        $('#updateApp').click(function () {
                
            $.confirm({
                title: 'warning!',
                content: 'Are you sure you want to install',
                buttons: {
                    approve: {
                        text: 'Yes Install',
                        btnclass: 'btn-success',
                        keys: ['enter', 'shift'],
                        action: function () {
                            var versionname = versions[0].version_name;

                            var obj = {};
                            obj.links = '/version/save';
                            obj.requestedversion = versionname;

                            $.ajax({
                                type: "POST",
                                url: "/oms/Management/projectmainpage.aspx/saveversiondetails",
                                contentType: "application/json; charset=utf-8",
                                data: JSON.stringify(obj),
                                dataType: "json",
                                success: function (data) {

                                    console.log(data)
                                    $.alert({
                                        title: 'message',
                                        content: data.d,
                                    });
                                }
                            });
                        }
                    },
                    reject: {
                        text: 'No Next time may be',
                        btnclass: 'btn-danger',
                        keys: ['enter', 'shift'],
                        action: function () {
                                
                        }
                    }
                }
            });
        });
            


        $('.ico').click (function () {
            $('.ico').addClass('liked');
        });
        $('#btnHelpPopupRec').click(function () {
            $('#panelForQuick, .backdrop').removeClass('off');

        });
        $('.quickCross').click(function () {
            $('#panelForQuick, .backdrop').addClass('off');
            $('.ico').removeClass('liked');
        });
        $('#editFav').click(function () {
            $('#panelForQuick .itm').toggleClass('editable');
        });
        $('body').on('click', '#boxedFavData .pathLoader', function () {
                
            var Rurl = '/' + $(this).data('url');
            console.log(Rurl);
            openMenuFromNav(Rurl)
        });
            
        $('body').on('click', '.delTag', function () {
            var rUrl = $(this).data('url');
            var titl = $(this).data('titl');
                

            $.confirm({
                title: 'Warning!',
                content: 'Are you sure you want to remove the item from Favourite!',
                buttons: {
                    confirm: function () {
                        favDelete(rUrl, titl);
                    },
                    cancel: function () {
                        //$.alert('Canceled!');
                    }
                }
            });
        });
            

        if (localStorage.getItem("fv") === null || typeof localStorage.fv === 'undefined') {
            //...
            var obj = {};
            obj.url = ''
            $.ajax({
                type: "POST",
                url: "/oms//Management/ProjectMainPage.aspx/GetFav",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(obj),
                dataType: "json",
                success: function (data) {
                    var url = window.location.href;
                    var spUrl = url.split("/")
                    var a = "";
                    for (i = 0; i < spUrl.length; i++) {
                        if (i > 2) {
                            if (i == spUrl.length - 1) {
                                a += spUrl[i] + ""
                            } else {
                                a += spUrl[i] + "/"
                            }

                        }
                    }
                    //console.log(data.d)
                    var items = data.d;
                    generateFavitems(items)
                    for (i = 0 ; i < items.length; i++) {
                        if (items[i].PAGE_URL == a) {
                            $('#FavAdd').hide();
                                
                        } else {
                            $('#FavAdd').show()

                        }
                    }
                }
            });
        } else {
            var url = window.location.href;
            var spUrl = url.split("/")
            var a = "";
            for (i = 0; i < spUrl.length; i++) {
                if (i > 2) {
                    if (i == spUrl.length - 1) {
                        a += spUrl[i] + ""
                    } else {
                        a += spUrl[i] + "/"
                    }

                }
            }
            var items = JSON.parse(localStorage.getItem('fv'))
            console.log('from storage',items)
            generateFavitems(items);
            for (i = 0 ; i < items.length; i++) {
                if (items[i].PAGE_URL == a) {
                    //alert(a)
                    $('#FavAdd').hide();
                       
                } else {
                    $('#FavAdd').show()
                }
            }
        }
        //Get Fav
            
        $('#FavAdd').click(function () {

            var url = window.location.href;
            var spUrl = url.split("/")
            var a = "";
            for (i = 0; i < spUrl.length; i++) {
                if (i > 2) {
                    if (i == spUrl.length - 1) {
                        a += spUrl[i] + ""
                    } else {
                        a += spUrl[i] + "/"
                    }

                }
            }
            $('#addfavModal').modal('show');
            var tl = $('.panel-heading> .panel-title>h3').text();
            $('#titleFav').val(tl);
            //$('#pgLink').val(a).attr('disabled');
        });
            
    });
function generateFavitems(items) {
    var htmlItems = '<div class="flexRow">';
    for (i = 0; i < items.length; i++) {
        htmlItems += '<div class="itm redPage" >'
        htmlItems += '<span class="delTag" data-url="' + items[i].PAGE_URL + '" data-titl="' + items[i].PAGE_TITLE + '"><i class="fa fa-close"></i></span>'
        htmlItems += '<div class="pathLoader" data-url="' + items[i].PAGE_URL + '"><div class="icn"><i class="fa fa-link"></i></div>'
        htmlItems += '<div class="tx">' + items[i].PAGE_TITLE + '</div>'
        htmlItems += ' </div></div>'
    }
    htmlItems += '</div>'
    $('#boxedFavData').html(htmlItems);
    if (items.length == 10) {
        $('#FavAdd').hide()
    } else {
        $('#FavAdd').show()
    }
    localStorage.setItem('fv', JSON.stringify(items))

}
function saveFav(){
    var url = window.location.href;
    var spUrl = url.split("/")
    var a = "";
    for (i = 0; i < spUrl.length; i++) {
        if (i > 2) {
            if (i == spUrl.length -1)
            {
                a += spUrl[i] + ""
            } else {
                a += spUrl[i] + "/"
            }
                    
        }
    }
    var addObj = {};
    addObj.url = a;
    addObj.title = $('#titleFav').val();
    console.log(addObj);
    $.ajax({
        type: "POST",
        url: "/oms/Management/ProjectMainPage.aspx/addFav",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(addObj),
        dataType: "json",
        success: function (data) {
            var items = data.d;

            var htmlItems = '<div class="flexRow">';
            for (i = 0; i < items.length; i++) {
                htmlItems += '<div class="itm redPage" data-url="' + items[i].PAGE_URL + '">'
                htmlItems += '<span class="delTag" data-url="' + items[i].PAGE_URL + '" data-titl="' + items[i].PAGE_TITLE + '"><i class="fa fa-close"></i></span>'
                htmlItems += '<div class="pathLoader" data-url="' + items[i].PAGE_URL + '"><div class="icn"><i class="fa fa-link"></i></div>'
                htmlItems += '<div class="tx">' + items[i].PAGE_TITLE + '</div>'
                htmlItems += ' </div></div>'
            }
            htmlItems += '</div>'
            $('#boxedFavData').html(htmlItems);
            if (items.length == 10) {
                $('#FavAdd').hide()
            } else {
                $('#FavAdd').show()
            }
            $('#titleFav').val('')
            localStorage.setItem('fv', JSON.stringify(items))
            for (i = 0 ; i < items.length; i++) {
                if (items[i].PAGE_URL == a) {
                    $('#FavAdd').hide();
                            
                } else {
                    $('#FavAdd').show()
                           
                }
            }
            $('#addfavModal').modal('hide');
            $.alert({
                title: 'Alert!',
                content: 'Favourite Added Successfully!',
            });

        }
    });
}
function favDelete(rUrl, titl) {
    var url = window.location.href;
    var spUrl = url.split("/")
    var a = "";
    for (i = 0; i < spUrl.length; i++) {
        if (i > 2) {
            if (i == spUrl.length - 1) {
                a += spUrl[i] + ""
            } else {
                a += spUrl[i] + "/"
            }

        }
    }
    var RObj = {};
    RObj.url = rUrl;
    RObj.title = titl;
    console.log(RObj);
    $.ajax({
        type: "POST",
        url: "/oms/Management/ProjectMainPage.aspx/RemoveFav",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(RObj),
        dataType: "json",
        success: function (data) {
            var items = data.d;

            var htmlItems = '<div class="flexRow">';
            for (i = 0; i < items.length; i++) {
                htmlItems += '<div class="itm redPage" data-url="' + items[i].PAGE_URL + '">'
                htmlItems += '<span class="delTag" data-url="' + items[i].PAGE_URL + '" data-titl="' + items[i].PAGE_TITLE + '"><i class="fa fa-close"></i></span>'
                htmlItems += '<div class="pathLoader" data-url="' + items[i].PAGE_URL + '"><div class="icn"><i class="fa fa-link"></i></div>'
                htmlItems += '<div class="tx">' + items[i].PAGE_TITLE + '</div>'
                htmlItems += ' </div></div>'
            }
            htmlItems += '</div>'
            $('#boxedFavData').html(htmlItems);
            if (items.length == 10) {
                $('#FavAdd').hide()
            } else {
                $('#FavAdd').show()
            }
            localStorage.setItem('fv', JSON.stringify(items))
            for (i = 0 ; i < items.length; i++) {
                if (items[i].PAGE_URL == a) {
                    $('#FavAdd').hide();
                } else {
                    $('#FavAdd').show()
                }
            }
        }
    });
}    
//End start
    function myMenuFilter() {
            
            
        // Declare variables
        var input, filter, ul, li, a, i, txtValue;
        input = document.getElementById('searchOnMenu');
        filter = input.value.toUpperCase();
        ul = document.getElementById("clonedMenus");
        li = ul.getElementsByTagName('li');
        //console.log(filter);
        //if (filter != "") {
        //    $('#clonedMenus').show();
        //} else {
        //    $('#clonedMenus').hide();
        //}
        // Loop through all list items, and hide those who don't match the search query
        for (i = 0; i < li.length; i++) {
            a = li[i].getElementsByTagName("a")[0];
            txtValue = a.textContent || a.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                li[i].style.display = "";
            } else {
                li[i].style.display = "none";
            }
        }
    }
    function myMenuFilter2() {
        console.log("hi")
        // Declare variables
        var input, filter, ul, li, a, i, txtValue;
        input = document.getElementById('searchOnMenu2');
        filter = input.value.toUpperCase();
        ul = document.getElementById("mApp");
        li = ul.getElementsByTagName('li');
        //console.log(filter);
        //if (filter != "") {
        //    $('#clonedMenus').show();
        //} else {
        //    $('#clonedMenus').hide();
        //}
        // Loop through all list items, and hide those who don't match the search query
        for (i = 0; i < li.length; i++) {
            a = li[i].getElementsByTagName("a")[0];
            txtValue = a.textContent || a.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                li[i].style.display = "";
            } else {
                li[i].style.display = "none";
            }
        }
    }
$(document).ready(function () {
    var isChat = $('#hdnChat').val();
    if (isChat == '1') {
        $('#chatUSer').removeClass('hide');
    } else {
        $('#chatUSer').addClass('hide');
    }
    //$('#clonedMenus').hide();
    $("#searchOnMenu").focus(function () {
        $(this).parent().parent('.searchMenuCont').find('.search-result').addClass('show');
    }).blur(function () {
        $(this).parent().parent('.searchMenuCont').find('.search-result').removeClass('show')
    });

    $('.scrollBarSearhMenu').slimScroll({
        height: '450px'
    });
    setTimeout(function () {
        $("#clonedMenus li").each(function (id, elem) {
                    
            var ss = $(elem).find('.ss');
            //console.log(ss);
            if ($(ss).text() == " ") {
                $(ss).css({ 'margin-top': '6px' });
            }
        });
    }, 1000)
    $(".sidenav>ul li").each(function (id, elem) {
               
        var classes = ['gogreen', 'mblue', 'Cpink', 'delta', 'alphaE', 'ramGr', 'ieCle', 'samsoon', 'regxs'];

        if (!$(elem).children("ul").length) {
            var pMainModule = $(elem).closest('ul.dropdown-menu.firstSub').parent('li').addClass('theItem').children('a').text();

            var arrTick = "";
            if (pMainModule != "") {
                arrTick = ">"
            } else {
                arrTick = ""
            }

            var pText = $(elem).closest('ul.dropdown-menu').parent('li').addClass('theItem').children('a').text();
            var e = $(elem).clone();
            var s = $(elem).clone();
            var index = Math.floor(Math.random() * classes.length);
            var className = classes[index];
            var itemText = $(e).find('a').text();
            var itemText2 = $(s).find('a').text();
            var finalItem = $(e).addClass(className).find('a').html('<span class="ss"> ' + pMainModule + arrTick + pText + '</span><span class="texts"> ' + itemText + '</span>');
            var finalItem2 = $(s).addClass(className).find('a').html('<span class="texts"> ' + itemText + '</span>')
            //$(e).addClass(className);
            $('#clonedMenus').append(e);
            $('#mApp').append(s);
        }
    });
    //$('#updateModal').modal('show');
            
});






