$(document).ready(function () {

    // show chart on icon click
    $('.showChart').click(function () {

        var classChek = $(this).attr('class');

        if ($(this).hasClass('activechart')) {
            $('.chartSec').addClass('hide');
            $('.tableInfo').removeClass('hide');
            console.log('hide chart');
            $(this).removeClass('activechart');
        } else {
            $(this).addClass('activechart');
            $('.tableInfo').addClass('hide');
            $('.chartSec').removeClass('hide');
        }
    })
    //end


    var toggle = document.querySelector('.toggle input')
    $('.toggler').on('click', function (e) {
        e.preventDefault();
        toggle.checked = !toggle.checked;
        //toggle.prop("checked", !toggle.prop("checked"));
        $('body').toggleClass('navactive');
    });
 

    $('#toDate').datepicker('setDate', today);

    $('.calToggleicon').click(function () {
        $(this).parent('.calwrap').find('.datepicker').datepicker().focus();
    });


});