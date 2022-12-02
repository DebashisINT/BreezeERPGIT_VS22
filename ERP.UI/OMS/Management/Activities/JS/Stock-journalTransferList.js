
$(document).ready(function () {
    //debugger;
    document.onkeydown = function (e) {
        if (event.altKey == true) {
            switch (event.keyCode) {

                case 97:
                    StopDefaultAction(e);
                    if (getUrlVars().req != "V") {
                        var url = 'Stock-JournalTransfer.aspx?key=' + 'ADD';
                        window.location.href = url;
                    }
                    break;
                case 65:
                    StopDefaultAction(e);
                    if (getUrlVars().req != "V") {
                        var url = 'Stock-JournalTransfer.aspx?key=' + 'ADD';
                        window.location.href = url;
                    }
                    break;

            }
        }
    }
});

function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
}
function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}

function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };
    e.returnValue = false;
    e.stopPropagation();
}

function OnClickDelete(keyValue) {
    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
        if (r == true) {
            gridjournal.PerformCallback('Delete~' + keyValue);
            gridjournal.PerformCallback('Display');
        }
    });
}

function OnMoreInfoClick(keyValue) {
    var url = 'Stock-JournalTransfer.aspx?key=' + keyValue + '&type=REPLACE';
    window.location.href = url;
}

function OnAddButtonClick() {
    var url = 'Stock-JournalTransfer.aspx?key=' + 'ADD';
    window.location.href = url;
}

function cListPanelEndCall() {
    if (gridjournal.cpDelete != null) {
        jAlert(gridjournal.cpDelete);
    }
}

//Function for Date wise filteration
function updateGridByDate() {

    if (cFormDate.GetDate() == null) {
        jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
    }
    else if (ctoDate.GetDate() == null) {
        jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
    }

    else {

        gridjournal.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd'));

        gridjournal.Refresh();


    }
}
//End
function gridRowclick(s, e) {
    $('#gridjournal').find('tr').removeClass('rowActive');
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
                console.log(value);
                $(value).css({ 'opacity': '1' });
            }, 100);
        });
    }, 200);
}

    $(document).ready(function () {
        if ($('body').hasClass('mini-navbar')) {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 90;
            gridjournal.SetWidth(cntWidth);
        } else {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 220;
            gridjournal.SetWidth(cntWidth);
        }
        $('.navbar-minimalize').click(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                gridjournal.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                gridjournal.SetWidth(cntWidth);
            }

        });
    });
