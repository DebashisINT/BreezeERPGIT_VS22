$(document).ready(function(){
    "use strict";

    if ($('.bgImage').data('image') != '') {
       var dataBg = $('.bgImage').data('image');
       $('.bgImage').css({'background-image' : 'url(' + dataBg + ')' });
    }else {
       $('.bgImage').css({'background-image' : 'url(assets/img/bglogin.jpg)' });
    }


    // search field show hide
    $('#showSearch').on('click', function(e){
        e.preventDefault();
        if($(this).hasClass('clicked')){
            $(this).removeClass('clicked');
            $('.srcfield .text').removeClass('hide');
            $('.srcfield .srchinput').addClass('hide');
        }else{
          $(this).addClass('clicked');
          $('.srcfield .text').addClass('hide');
          $('.srcfield .srchinput').removeClass('hide');
          $('.srcfield .srchinput input[type="text"]').focus();
        }
    });


    function windowSizer() {
      var winH = $(window).height(),
          headerH = $('#header').height(),
          subH = winH - headerH ;
      $('.rightSide, .mainWraper').css({ 'min-height': subH });
    }
    windowSizer();
    $(window).resize(function(){
        windowSizer();
    });

    // Add body-small class if window less than 768px
    if ($(this).width() < 769) {
        $('body').addClass('mini-navbar')
    } else {
        $('body').removeClass('mini-navbar')
    }
    // Minimalize menu when screen is less than 768px
    $(window).bind("resize", function () {
        if ($(this).width() < 769) {
            $('body').addClass('mini-navbar')
        } else {
            $('body').removeClass('mini-navbar')
        }
    });
    // Minimalize menu
    $('.navbar-minimalize').click(function (e) {
        $("body").toggleClass("mini-navbar");
       
        if ($("body").hasClass("mini-navbar")) {
            localStorage.setItem("LCmini-navbarcss", "Yes");
        }
        else {
            localStorage.setItem("LCmini-navbarcss", "No");
        }


    });
    //$('.data-table').DataTable();
    $(function () {
      $('[data-toggle="tooltip"]').tooltip()
    });
    $('.Closer').click(function () {
        $('.rgth').toggleClass('full');
        $('.ltfg').toggleClass('close');
        if ($('.Closer').find('.fa').hasClass('fa-angle-right')) {
            $('.Closer ').find('.fa').removeClass('fa-angle-right').addClass('fa-angle-left');
            //alert('left');
        } else {
            $('.Closer .fa').addClass('fa-angle-right');
        }
    });


    //Menu function

    $('.sidenav>ul>li a').click(function (e) {
        //e.preventDefault();
        //alert('first Level Clicked');
        $('.sidenav ul.dropdown-menu').hide();
        $('.sidenav>ul>li a').removeClass('active');
        $('.seclevel>li').removeClass('active');
        $(this).addClass('active');
        $(this).next('ul.dropdown-menu').show();
        $(this).next('ul.dropdown-menu').find('.seclevel li:first').find('.thrdlevel').show();
        $(this).next('ul.dropdown-menu').find('.seclevel li:first').addClass('active');
        $(this).addClass('active');
    });
    //mouseenter function
    $('.seclevel>li').mouseenter(function (e) {
        $('.thrdlevel').hide();
        $('.seclevel>li').removeClass('active');
        $(this).addClass('active');
        $(this).find('.thrdlevel').show();
    });
    $('.seclevel>li>a').click(function () {
        
        if ($(this).hasAttr("href")) {
            
        } else {
            alert('hi');
            //do something
            $(this).closest('ul.dropdown-menu').show();
            $(this).next('ul.dropdown-menu').show();
        }
    });
    $('.rightSide, #header').click(function (e) {
        //alert('hi');
        $('.sidenav ul.dropdown-menu').hide();
        $('.sidenav>ul>li a').removeClass('active');
    });
    $('iframe').load(function () {
        $(this).contents().find("body").on('click', function (event) {
            $('.sidenav ul.dropdown-menu').hide();
            $('.sidenav>ul>li a').removeClass('active');
        });
    });
    //Link hover url showing disabled
    //setTimeout(function () {

    //    $('.sidenav a[href]').each(function () {
    //        var href = this.href;

    //        $(this).removeAttr('href').css('cursor', 'pointer').click(function () {
    //            if (href.toLowerCase().indexOf("#") >= 0) {

    //            } else {
    //                window.open(href, '_self');
    //            }
    //        });
    //    });

    //}, 500);
    if (!$('.form_main').hasClass('clearfix')) {
        $('.form_main').addClass('clearfix');
    }
    if ($(".scrollHorizontal").length) {
        $(".scrollHorizontal").mCustomScrollbar({
            axis: "x",
            theme: "rounded-dots",
            scrollInertia: 5,
            autoExpandScrollbar: true,
            advanced: { autoExpandHorizontalScroll: true }
        });
    }

    
});