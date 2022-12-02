$(document).ready(function () {
    $('a .white').click(function (e) {
        e.preventDefault();
    });

    $('[data-toggle="tooltip"]').tooltip();
    $('[data-toggle="popover"]').popover({ html: true });


    $('.zoom').click(function (e) {
        $('.arrowPointer').remove();
        $('.zoom').removeClass('DisableClass');
        var divid = $(this).attr('data-click');
        this.className = this.className + ' DisableClass';
        $('.panelClass').hide();
        $('.panelClass').css({ top: '50px', opacity: '0' });
        $('#' + divid).show();
        $('#' + divid).animate({ top: '0px', opacity: '1' });

        this.children[0].children[0].innerHTML = this.children[0].children[0].innerHTML + '<i class="far fa-hand-point-left arrowPointer"></i>';


        if (divid == "PendingDiv") {
            cgridpact.Refresh();
        } else if (divid == "totFollowupDiv") {
            cgridEF.Refresh();
        } else if (divid == "closedFollowupDiv") {
            cgridClosed.Refresh();
        } else if (divid == "ConversionDiv") {
            cratio.Refresh();
        } else if (divid == "followhisDiv") {
            cgridHistory.Refresh();
        } else if (divid == "followusingdiv") {
            cgridusing.Refresh();
        }
    });

});


function showCustomNewNotify(text) {
    var x = document.getElementById("CustomNewNotify");
    x.innerHTML = text;
    x.className = "show";
    setTimeout(function () { x.className = x.className.replace("show", ""); }, 6000);
}



function RefreshWidget() {
    if (document.getElementsByClassName('DisableClass').length > 0) {

        if (document.getElementsByClassName('DisableClass')[0].id == 'totFollowup') {
            LoadTopSaleman();
        }
        if (document.getElementsByClassName('DisableClass')[0].id == 'Conversion') {
            LoadRatio();
        }

    }
}


function LoadTopSaleman() { 
    cgridEF.Refresh();
}

function LoadRatio() {
    cratio.Refresh();
}