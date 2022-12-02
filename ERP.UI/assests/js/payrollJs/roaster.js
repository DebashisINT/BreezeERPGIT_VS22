
    function AddRoster() {
        var url = '/Roster/Index?ActionType=ADD';
        window.location.href = url;
    }

    function OnClickEditForm(RosterId) {
        var url = '/Roster/Index?ActionType=EDIT&RosterId=' + RosterId;
        window.location.href = url;
    }
function OnClickCopy(RosterId) {
    var url = '/Roster/Index?ActionType=Copy&RosterId=' + RosterId;
    window.location.href = url;
}
function OnClickDeleteForm(id) {
    var url = '/Roster/DeleteRoster/';

    jConfirm("Confirm Delete?", "Confirmation Dialog", function (ret) {
        if (ret == true) {
            $.ajax({
                type: "POST",
                url: url,
                data: JSON.stringify({ 'ActionType': 'Delete', 'id': id }),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    console.log(response);

                    if (response.Success) {
                        jAlert(response.Message);
                        GVPartialRoster.Refresh();
                    }
                    else {
                        jAlert("Please try again later!");
                    }
                },
                error: function (response) {
                    console.log(response);
                }
            });
        }
    });
}
function gridRowclick(s, e) {
    $('#GVPartialRoster').find('tr').removeClass('rowActive');
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
