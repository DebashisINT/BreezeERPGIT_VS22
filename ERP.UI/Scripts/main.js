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
        $('body').addClass('body-small')
    } else {
        $('body').removeClass('body-small')
    }
    // Minimalize menu when screen is less than 768px
    $(window).bind("resize", function () {
        if ($(this).width() < 769) {
            $('body').addClass('body-small')
        } else {
            $('body').removeClass('body-small')
        }
    });
    // Minimalize menu
    //$('.navbar-minimalize').click(function (e) {
    //    $("body").toggleClass("mini-navbar");
    //    SmoothlyMenu();
      
    //});
    //$('.data-table').DataTable();
    $(function () {
      $('[data-toggle="tooltip"]').tooltip()
    });
});